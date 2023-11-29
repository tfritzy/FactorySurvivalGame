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
        inventory = props.inventory;
        onSlotMouseUp = props.onSlotMouseUp;
        onSlotMouseHold = props.onSlotMouseHold;

        style.justifyContent = Justify.SpaceBetween;

        InitWornSection();
        InitBackpackGrid();
    }

    private void InitWornSection()
    {
        wornItemsSection = new WornItemsSection();
        wornItemsSection.style.marginBottom = 80;
        Content.Add(wornItemsSection);
    }

    private void InitBackpackGrid()
    {
        var backpackSection = new VisualElement();

        inventoryGrid = new InventoryGrid(new InventoryGrid.Props
        {
            inventory = inventory,
            onSlotMouseUp = onSlotMouseUp,
            onSlotMouseHold = onSlotMouseHold
        });

        backpackSection.Add(inventoryGrid);
        Content.Add(backpackSection);
    }

    public override void Update()
    {
        inventoryGrid.Update();
        wornItemsSection.Update();
    }
}