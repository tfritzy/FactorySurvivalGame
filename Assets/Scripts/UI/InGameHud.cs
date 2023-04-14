using Core;
using UnityEngine.UIElements;

public class InGameHud : ActiveElement
{
    private InventoryDrawer inventoryDrawer;
    private InventoryComponent selectedSourceInventory;
    private int selectedSourceIndex;

    public InGameHud()
    {
        this.style.width = Length.Percent(100);
        this.style.height = Length.Percent(100);

        Character character = new Dummy(Managers.World.Context);
        InventoryComponent inventory = new InventoryComponent(character, 200);
        inventory.AddItem(new Stone(), 1);
        inventory.AddItem(new Stone(), 8);
        inventory.AddItem(new Stone(), 12);
        inventory.AddItem(new Stone(), 25);

        this.inventoryDrawer = new InventoryDrawer(new InventoryDrawer.Props
        {
            inventory = inventory,
            onSelectSlot = this.OnSelectSlot
        });
        this.Add(this.inventoryDrawer);
    }

    public override void Update()
    {
        this.inventoryDrawer.Update();
    }

    private void OnSelectSlot(InventoryComponent inventory, int index)
    {
        if (selectedSourceInventory == null)
        {
            selectedSourceInventory = inventory;
            selectedSourceIndex = index;
        }
        else
        {
            selectedSourceInventory.TransferIndex(inventory, selectedSourceIndex, index);
            selectedSourceInventory = null;
        }
    }
}