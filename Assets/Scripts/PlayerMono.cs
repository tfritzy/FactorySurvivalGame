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

    void Start()
    {
        this.Actual = new Player(Managers.World.Context, 0);
        this.SelectedInventory = this.Actual.GetComponent<ActiveItems>();
        this.SelectedInventoryIndex = 0;
    }

    void Update()
    {
        ListenForInventoryControls();
        PreviewSelectedItem();
    }

    private void PreviewSelectedItem()
    {
        if (PreviewBuilding != null)
        {
            return;
        }

        if (this.SelectedItem == null)
        {
            return;
        }

        if (this.SelectedItem.Builds != null)
        {
            PreviewBuilding =
                (Building)Character.Create(
                    this.SelectedItem.Builds.Value,
                    WorldMono.Instance.Context,
                    this.Actual.Alliance);
            WorldMono.Instance.World.AddBuilding(PreviewBuilding, new Point2Int(0, 0));
        }
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