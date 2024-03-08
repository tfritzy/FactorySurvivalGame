using System;
using Core;
using UnityEngine;
using UnityEngine.UIElements;

public class ActiveItemsBar : ActiveElement
{
    private InventoryGrid inventoryGrid;

    public struct Props
    {
        public Action<MouseUpEvent, Inventory, int> onSlotMouseUp;
        public Action<MouseMoveEvent, Inventory, int> onSlotMouseHold;
        public Action onInventoryButtonClicked;
        public Action onCraftingButtonClicked;
    }
    public ActiveItemsBar(Props props)
    {
        style.marginBottom = 10;

        this.Content.style.backgroundColor = ColorTheme.Current.PanelBackgroundColor;
        this.Content.SetAllBorderRadius(InventorySlot.BorderRadius);
        this.Content.SetAllBorderWidth(1);
        this.Content.SetAllBorderColor(ColorTheme.Current.PanelOutline);

        var content = new VisualElement();
        this.Content.Add(content);
        this.Content = content;

        // InitButtons(props);
        InitInventory(props);
    }

    private void InitButtons(Props props)
    {
        var buttonContainer = new VisualElement();
        this.Content.Add(buttonContainer);
        buttonContainer.style.height = Length.Percent(100);

        var craftingButton = new Button();
        craftingButton.clicked += props.onCraftingButtonClicked;
        craftingButton.style.backgroundColor = Color.cyan;
        craftingButton.style.width = InventorySlot.Size;
        craftingButton.style.height = Length.Percent(50);
        craftingButton.SetAllMargin(0);
        buttonContainer.Add(craftingButton);

        var inventoryButton = new Button();
        inventoryButton.clicked += props.onInventoryButtonClicked;
        inventoryButton.style.backgroundColor = Color.cyan;
        inventoryButton.style.width = InventorySlot.Size;
        inventoryButton.style.height = Length.Percent(50);
        inventoryButton.SetAllMargin(0);
        buttonContainer.Add(inventoryButton);
    }

    private void InitInventory(Props props)
    {
        Inventory inventory = PlayerMono.Instance.Actual.GetComponent<ActiveItems>();

        // inventory.AddItem(new LimestoneBrick(32), 1);
        // inventory.AddItem(new LimestoneDoubleBrick(32), 2);
        // inventory.AddItem(new SorterItem(8), 3);
        // inventory.AddItem(new ClayFurnaceItem(32), 4);

        inventory.AddItem(new ConveyorItem(16), 5);
        inventory.AddItem(new ConveyorItem(16), 6);
        inventory.AddItem(new MineshaftItem(1), 7);
        // inventory.AddItem(new DepotItem(1), 8);
        // inventory.AddItem(new Limestone(7), 10);

        this.inventoryGrid = new InventoryGrid(new InventoryGrid.Props
        {
            inventory = inventory,
            onSlotMouseUp = props.onSlotMouseUp,
            onSlotMouseHold = props.onSlotMouseHold,
        });
        this.Content.Add(this.inventoryGrid);
    }

    public override void Update()
    {
        this.inventoryGrid.Update();
    }
}