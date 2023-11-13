using Core;
using UnityEngine;

public class PlayerMono : MonoBehaviour
{
    public Inventory SelectedInventory;
    public int SelectedInventoryIndex;
    public Player Actual;

    private Item? SelectedItem => this.SelectedInventory.GetItemAt(this.SelectedInventoryIndex);
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
        this.Actual = new Player(WorldMono.Instance.Context, 0);
        this.SelectedInventory = this.Actual.GetComponent<ActiveItems>();
        this.SelectedInventoryIndex = 0;
    }

    void Update()
    {
        ListenForInventoryControls();
        PreviewSelectedItem();
        BuildPreviewBuilding();
    }

    private void PreviewSelectedItem()
    {
        if (this.SelectedItem == null)
        {
            return;
        }

        if (this.SelectedItem.Builds != null)
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
                this.Actual.BuidPreviewBuildingFromItem(
                    this.SelectedInventoryIndex,
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

        this.Actual.MakePreviewBuildingRealFromItem(
            this.SelectedInventoryIndex,
            PreviewBuilding);
        PreviewBuilding = null;
    }

    private void ListenForInventoryControls()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            this.SelectedInventoryIndex++;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            this.SelectedInventoryIndex--;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            this.SelectedInventoryIndex -= this.SelectedInventory.Width;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            this.SelectedInventoryIndex += this.SelectedInventory.Width;
        }

        if (this.SelectedInventoryIndex > this.SelectedInventory.Size)
        {
            this.SelectedInventoryIndex = this.SelectedInventoryIndex % this.SelectedInventory.Size;
        }

        if (this.SelectedInventoryIndex < 0)
        {
            this.SelectedInventoryIndex = this.SelectedInventory.Size + this.SelectedInventoryIndex;
        }
    }
}