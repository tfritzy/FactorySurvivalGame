using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

public class PlayerMono : MonoBehaviour
{
    public Inventory SelectedInventory;
    public int SelectedInventoryIndex;
    public Player Actual;

    private Item? SelectedItem => SelectedInventory.GetItemAt(SelectedInventoryIndex);
    private Dictionary<Point2Int, Building> previewBuildings = new();
    private Dictionary<Point2Int, GameObject> conveyorArrows = new();
    private BuildGrid buildGrid;
    private HexSide rotation = 0;

    private static PlayerMono instance;
    public static PlayerMono Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<PlayerMono>();
            }
            return instance;
        }
    }

    void Awake()
    {
        Actual = new Player(WorldMono.Instance.Context, 0);
        SelectedInventory = Actual.GetComponent<ActiveItems>();
        SelectedInventoryIndex = 0;
        InputManager.Instance.RegisterKeyDown(KeyCode.RightArrow, () => IncrementInventoryIndex(1));
        InputManager.Instance.RegisterKeyDown(KeyCode.LeftArrow, () => IncrementInventoryIndex(-1));
        InputManager.Instance.RegisterKeyDown(
            KeyCode.UpArrow,
            () => IncrementInventoryIndex(-SelectedInventory.Width));
        InputManager.Instance.RegisterKeyDown(
            KeyCode.DownArrow,
            () => IncrementInventoryIndex(SelectedInventory.Width));

#if UNITY_EDITOR
        Cursor.SetCursor(UIElements.GetElement(UIElementType.Cursor).texture, Vector2.zero, CursorMode.ForceSoftware);
#endif
    }

    void Update()
    {
        PreviewSelectedItem();
        BuildPreviewBuildings();
        RotatePreviewBuilding();
    }

    private void PreviewSelectedItem()
    {
        if (SelectedItem == null)
        {
            if (previewBuildings.Count > 0)
            {
                ClearPreviewBuildings();
            }

            return;
        }

        if (previewBuildings.Count > 0 && SelectedItem?.Builds != previewBuildings.Values.First()?.Type)
        {
            ClearPreviewBuildings();
        }

        if (SelectedItem.Builds != null)
        {
            Point3Int? hex = RaycastHelper.GetHexUnderCursor();
            if (hex == null ||
                previewBuildings.ContainsKey((Point2Int)hex) ||
                !WorldMono.Instance.World.Terrain.IsInBounds(hex.Value))
            {
                return;
            }

            if (previewBuildings.Count > 0 && !Input.GetMouseButton(0))
            {
                ClearPreviewBuildings();
            }

            if (buildGrid == null)
            {
                buildGrid = new GameObject("BuildGrid").AddComponent<BuildGrid>();
            }
            else
            {
                buildGrid.gameObject.SetActive(true);
            }

            buildGrid.SetPos(hex.Value);
            var building = Actual.BuidPreviewBuildingFromItem(
                SelectedInventoryIndex,
                (Point2Int)hex.Value);
            if (building != null)
            {
                var actualBuilding = WorldMono.Instance.World.GetBuildingAt((Point2Int)hex.Value);
                actualBuilding.SetRotation(rotation);
                previewBuildings.Add((Point2Int)hex.Value, actualBuilding);
                UpdateArrows();
            }
        }
    }

    private void UpdateArrows()
    {
        ClearArrows();

        foreach (Building building in previewBuildings.Values)
        {
            if (building.Conveyor != null)
            {
                var arrow = Instantiate(DecalLoader.GetDecalPrefab(DecalLoader.Decal.Arrow));
                arrow.transform.position = WorldConversions.HexToUnityPosition(building.GridPosition);
                arrow.transform.rotation = Quaternion.Euler(0, 60 * (int)building.Rotation, 0);
                conveyorArrows.Add((Point2Int)building.GridPosition, arrow);
            }
        }
    }

    private void ClearArrows()
    {
        foreach (var arrow in conveyorArrows.Values)
        {
            Destroy(arrow);
        }
        conveyorArrows.Clear();
    }

    private void BuildPreviewBuildings()
    {
        if (previewBuildings.Count == 0)
        {
            return;
        }

        if (ClickLog.GetMouseButtonUp())
        {
            foreach (Building b in previewBuildings.Values)
            {
                Actual.MakePreviewBuildingRealFromItem(SelectedInventoryIndex, b);
            }
            previewBuildings.Clear();
            buildGrid.gameObject.SetActive(false);
            ClearArrows();
        }
    }

    private void ClearPreviewBuildings()
    {
        buildGrid.gameObject.SetActive(false);
        foreach (Building b in previewBuildings.Values)
        {
            if (b == null)
            {
                continue;
            }

            WorldMono.Instance.World.RemoveBuilding(b.Id);
        }
        previewBuildings.Clear();
    }

    private void RotatePreviewBuilding()
    {
        if (previewBuildings.Count == 0)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.R) || Input.mouseScrollDelta.y > 0)
        {
            rotation = GridHelpers.Rotate60(rotation);
            previewBuildings.Values.Last().SetRotation(rotation);
            UpdateArrows();
        }
        else if (Input.GetKeyDown(KeyCode.E) || Input.mouseScrollDelta.y < 0)
        {
            rotation = GridHelpers.Rotate60(rotation, clockwise: false);
            previewBuildings.Values.Last().SetRotation(rotation);
            UpdateArrows();
        }
    }

    private void IncrementInventoryIndex(int amount)
    {
        SelectedInventoryIndex += amount;
        if (SelectedInventoryIndex > SelectedInventory.Size)
        {
            SelectedInventoryIndex %= SelectedInventory.Size;
        }

        if (SelectedInventoryIndex < 0)
        {
            SelectedInventoryIndex = SelectedInventory.Size + SelectedInventoryIndex;
        }
    }
}