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
    }

    void Update()
    {
        ListenForInventoryControls();
        PreviewSelectedItem();
        BuildPreviewBuildings();
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

            previewBuildings.Add(
                (Point2Int)hex.Value,
                Actual.BuidPreviewBuildingFromItem(
                    SelectedInventoryIndex,
                    (Point2Int)hex.Value));
        }
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
                Actual.MakePreviewBuildingRealFromItem(
                    SelectedInventoryIndex,
                    b);
            }
            previewBuildings.Clear();
        }
    }

    private void ClearPreviewBuildings()
    {
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

    private void ListenForInventoryControls()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SelectedInventoryIndex++;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SelectedInventoryIndex--;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SelectedInventoryIndex -= SelectedInventory.Width;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SelectedInventoryIndex += SelectedInventory.Width;
        }

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