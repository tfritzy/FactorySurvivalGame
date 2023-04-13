using Core;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryDrawer : Drawer
{
    private InventoryGrid inventoryGrid;
    private WornItemsSection wornItemsSection;

    public InventoryDrawer()
    {
        this.style.justifyContent = Justify.SpaceBetween;
        this.style.backgroundColor = Color.white;
        this.SetAllBorderWidth(10);
        this.SetAllBorderColor(Color.black);

        InitWornSection();
        InitBackpackGrid();
    }

    private void InitWornSection()
    {
        this.wornItemsSection = new WornItemsSection();
        wornItemsSection.style.height = Length.Percent(30);
        this.Add(wornItemsSection);
    }

    private void InitBackpackGrid()
    {
        Character character = new Dummy(Managers.World.Context);
        InventoryComponent inventory = new InventoryComponent(character, 200);
        inventory.AddItem(new Stone(), 1);
        inventory.AddItem(new Stone(), 8);
        inventory.AddItem(new Stone(), 12);
        inventory.AddItem(new Stone(), 25);
        var backpackSection = new VisualElement();
        backpackSection.style.height = Length.Percent(60);
        backpackSection.Add(BuildButtonRow());

        this.inventoryGrid = new InventoryGrid(10, 20, inventory);
        backpackSection.Add(this.inventoryGrid);
        this.Add(backpackSection);
    }

    private VisualElement BuildButtonRow()
    {
        var buttonRow = new VisualElement();
        var dummyButton = new Button();
        dummyButton.text = "Dummy";
        buttonRow.Add(dummyButton);
        buttonRow.style.flexDirection = FlexDirection.Row;
        return buttonRow;
    }

    public override void Update()
    {
        this.inventoryGrid.Update();
        this.wornItemsSection.Update();
    }
}