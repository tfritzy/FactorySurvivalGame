using System;
using Core;
using UnityEngine;
using UnityEngine.UIElements;

public class ActiveItemsBar : ActiveElement
{
    private InventoryGrid inventoryGrid;
    private const int WIDTH = 10;
    private const int HEIGHT = 3;

    public struct Props
    {
        public Action<MouseUpEvent, InventoryComponent, int> onSlotMouseUp;
        public Action<MouseMoveEvent, InventoryComponent, int> onSlotMouseHold;
        public Action onInventoryButtonClicked;
        public Action onCraftingButtonClicked;
    }
    public ActiveItemsBar(Props props)
    {
        this.style.backgroundColor = Color.red;
        this.style.flexDirection = FlexDirection.Row;

        InitButtons(props);
        InitInventory(props);
    }

    private void InitButtons(Props props)
    {
        var buttonContainer = new VisualElement();
        this.Add(buttonContainer);
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
        var dummy = new Dummy(Managers.World.Context);
        InventoryComponent inventory = new InventoryComponent(dummy, 200);
        inventory.AddItem(new Stone(5), 1);
        inventory.AddItem(new Stone(7), 8);
        inventory.AddItem(new Stone(6), 12);
        inventory.AddItem(new Stone(7), 25);
        inventory.AddItem(new Stone(7), 35);
        inventory.AddItem(new Stone(7), 40);

        this.inventoryGrid = new InventoryGrid(new InventoryGrid.Props
        {
            width = WIDTH,
            height = HEIGHT,
            inventory = inventory,
            onSlotMouseUp = props.onSlotMouseUp,
            onSlotMouseHold = props.onSlotMouseHold,
        });
        this.Add(this.inventoryGrid);
    }

    public override void Update()
    {
        this.inventoryGrid.Update();
    }
}