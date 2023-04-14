using System;
using Core;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryDrawer : Drawer
{
    private InventoryGrid inventoryGrid;
    private WornItemsSection wornItemsSection;
    private Action<InventoryComponent, int> onSelectSlot;
    private InventoryComponent inventory;

    public struct Props
    {
        public InventoryComponent inventory;
        public Action<InventoryComponent, int> onSelectSlot;
    }

    public InventoryDrawer(Props props)
    {
        this.inventory = props.inventory;
        this.onSelectSlot = props.onSelectSlot;

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
        var backpackSection = new VisualElement();
        backpackSection.style.height = Length.Percent(60);
        backpackSection.Add(BuildButtonRow());

        this.inventoryGrid = new InventoryGrid(new InventoryGrid.Props
        {
            width = 10,
            height = 20,
            inventory = inventory,
            onSelectSlot = this.onSelectSlot
        });

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