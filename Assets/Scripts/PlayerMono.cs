using Core;
using UnityEngine;

public class PlayerMono : MonoBehaviour
{
    public Inventory SelectedInventory;
    public int SelectedInventoryIndex;
    public Player Actual;

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