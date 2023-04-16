using System;
using Core;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryDrawer : Drawer
{
    private InventoryGrid inventoryGrid;
    private WornItemsSection wornItemsSection;
    public Action<MouseUpEvent, InventoryComponent, int> onSlotMouseUp;
    public Action<MouseMoveEvent, InventoryComponent, int> onSlotMouseHold;
    private InventoryComponent inventory;

    public struct Props
    {
        public InventoryComponent inventory;
        public Action<MouseUpEvent, InventoryComponent, int> onSlotMouseUp;
        public Action<MouseMoveEvent, InventoryComponent, int> onSlotMouseHold;
    }

    public InventoryDrawer(Props props)
    {
        this.inventory = props.inventory;
        this.onSlotMouseUp = props.onSlotMouseUp;
        this.onSlotMouseHold = props.onSlotMouseHold;

        this.style.justifyContent = Justify.SpaceBetween;
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
        backpackSection.Add(BuildButtonRow());

        this.inventoryGrid = new InventoryGrid(new InventoryGrid.Props
        {
            width = 10,
            height = 14,
            inventory = inventory,
            onSlotMouseUp = this.onSlotMouseUp,
            onSlotMouseHold = this.onSlotMouseHold
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