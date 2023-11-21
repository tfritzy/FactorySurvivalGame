using Core;
using UnityEngine;

public class PlayerMono : MonoBehaviour
{
    public Inventory SelectedInventory;
    public int SelectedInventoryIndex;
    public Player Actual;

    private Item? SelectedItem => SelectedInventory.GetItemAt(SelectedInventoryIndex);
    private Building? PreviewBuilding;

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
        BuildPreviewBuilding();
    }

    private void PreviewSelectedItem()
    {
        if (SelectedItem == null)
        {
            return;
        }

        if (SelectedItem.Builds != null)
        {
            Point3Int? hex = RaycastHelper.GetHexUnderCursor();
            if (hex == null ||
                hex == PreviewBuilding?.GridPosition ||
                !WorldMono.Instance.World.Terrain.IsInBounds(hex.Value))
            {
                return;
            }

            if (PreviewBuilding != null)
            {
                WorldMono.Instance.World.RemoveBuilding(PreviewBuilding.Id);
                PreviewBuilding = null;
            }

            PreviewBuilding =
                Actual.BuidPreviewBuildingFromItem(
                    SelectedInventoryIndex,
                    (Point2Int)hex.Value);
        }
    }

    private void BuildPreviewBuilding()
    {
        if (PreviewBuilding == null)
        {
            return;
        }

        if (!Input.GetMouseButton(0))
        {
            return;
        }

        Actual.MakePreviewBuildingRealFromItem(
            SelectedInventoryIndex,
            PreviewBuilding);
        PreviewBuilding = null;
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