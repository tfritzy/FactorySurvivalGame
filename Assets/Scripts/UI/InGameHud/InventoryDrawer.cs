using System;
using Core;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryDrawer : Drawer
{
    private InventoryGrid inventoryGrid;
    private WornItemsSection wornItemsSection;
    public Action<MouseUpEvent, Inventory, int> onSlotMouseUp;
    public Action<MouseMoveEvent, Inventory, int> onSlotMouseHold;
    private Inventory inventory;

    public struct Props
    {
        public Inventory inventory;
        public Action<MouseUpEvent, Inventory, int> onSlotMouseUp;
        public Action<MouseMoveEvent, Inventory, int> onSlotMouseHold;
    }

    public InventoryDrawer(Props props)
    {
        this.inventory = props.inventory;
        this.onSlotMouseUp = props.onSlotMouseUp;
        this.onSlotMouseHold = props.onSlotMouseHold;

        this.style.justifyContent = Justify.SpaceBetween;

        InitWornSection();
        InitBackpackGrid();
    }

    private void InitWornSection()
    {
        this.wornItemsSection = new WornItemsSection();
        wornItemsSection.style.height = Length.Percent(30);
        wornItemsSection.style.marginBottom = 15;
        this.Content.Add(wornItemsSection);
    }

    private void InitBackpackGrid()
    {
        var backpackSection = new VisualElement();

        this.inventoryGrid = new InventoryGrid(new InventoryGrid.Props
        {
            inventory = inventory,
            onSlotMouseUp = this.onSlotMouseUp,
            onSlotMouseHold = this.onSlotMouseHold
        });

        backpackSection.Add(this.inventoryGrid);
        this.Content.Add(backpackSection);
    }

    public override void Update()
    {
        this.inventoryGrid.Update();
        this.wornItemsSection.Update();
    }
}