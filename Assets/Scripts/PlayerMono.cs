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
    private Building? previewBuilding;
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
        MakePreviewBuildingReal();
        RotatePreviewBuilding();
    }

    private void PreviewSelectedItem()
    {
        if (SelectedItem == null)
        {
            if (previewBuilding != null)
            {
                ClearPreviewBuilding();
            }

            return;
        }

        if (previewBuilding != null && SelectedItem?.Builds != previewBuilding?.Type)
        {
            ClearPreviewBuilding();
        }

        if (SelectedItem?.Builds != null)
        {
            Point3Int? hex = RaycastHelper.GetHexUnderCursor();
            if (hex == null ||
                previewBuilding?.GridPosition == hex ||
                !WorldMono.Instance.World.Terrain.IsInBounds(hex.Value))
            {
                return;
            }

            if (previewBuilding != null)
            {
                ClearPreviewBuilding();
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
                actualBuilding?.SetRotation(rotation);
                previewBuilding = actualBuilding;
                UpdateArrows();
            }
        }
    }

    private void UpdateArrows()
    {
        ClearArrows();

        if (previewBuilding?.Conveyor != null)
        {
            var arrow = Instantiate(DecalLoader.GetDecalPrefab(DecalLoader.Decal.Arrow));
            arrow.transform.position = WorldConversions.HexToUnityPosition(previewBuilding.GridPosition);
            arrow.transform.rotation = Quaternion.Euler(0, 60 * (int)previewBuilding.Rotation, 0);
            conveyorArrows.Add((Point2Int)previewBuilding.GridPosition, arrow);
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

    private void MakePreviewBuildingReal()
    {
        if (previewBuilding == null)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            Actual.MakePreviewBuildingRealFromItem(SelectedInventoryIndex, previewBuilding);
            previewBuilding = null;
            buildGrid.gameObject.SetActive(false);
            ClearArrows();
        }
    }

    private void ClearPreviewBuilding()
    {
        if (previewBuilding != null)
        {
            WorldMono.Instance.World.RemoveBuilding(previewBuilding.Id);
        }
        buildGrid.gameObject.SetActive(false);
        ClearArrows();
        previewBuilding = null;
    }

    private void RotatePreviewBuilding()
    {
        if (previewBuilding == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.R) || Input.mouseScrollDelta.y > 0)
        {
            rotation = GridHelpers.Rotate60(rotation);
            previewBuilding.SetRotation(rotation);
            UpdateArrows();
        }
        else if (Input.GetKeyDown(KeyCode.E) || Input.mouseScrollDelta.y < 0)
        {
            rotation = GridHelpers.Rotate60(rotation, clockwise: false);
            previewBuilding.SetRotation(rotation);
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