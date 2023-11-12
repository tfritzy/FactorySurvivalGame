using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.UIElements;

public class InGameHud : ActiveElement
{
    private InventoryDrawer inventoryDrawer;
    private ActiveItemsBar activeItemsBar;
    private CraftingMenu craftingMenu;
    private Inventory selectedSourceInventory;
    private int selectedSourceIndex;
    private SlotItemIcon hoveringSlot;
    private HashSet<int> touchedByDrag = new HashSet<int>();

    public InGameHud()
    {
        this.style.width = Length.Percent(100);
        this.style.height = Length.Percent(100);
        this.style.alignItems = Align.Center;
        this.style.flexDirection = FlexDirection.ColumnReverse;

        InitInventory();
        InitActionBar();
        InitCraftingMenu();
        InitHoveringSlot();
    }

    public override void Update()
    {
        this.inventoryDrawer.Update();
        this.activeItemsBar.Update();

        ListenForHotkeys();
    }

    private void InitInventory()
    {
        Character character = new Dummy(Managers.World.Context, 0);
        Inventory inventory = new Inventory(character, 10, 20);
        inventory.AddItem(new Stone(5), 1);
        inventory.AddItem(new Stone(7), 8);
        inventory.AddItem(new Stone(6), 12);
        inventory.AddItem(new Stone(7), 25);
        inventory.AddItem(new Stone(7), 35);
        inventory.AddItem(new Stone(7), 40);

        this.inventoryDrawer = new InventoryDrawer(new InventoryDrawer.Props
        {
            inventory = inventory,
            onSlotMouseUp = this.OnSlotMouseUp,
            onSlotMouseHold = this.OnSlotMouseHold,
        });
        this.Add(this.inventoryDrawer);
    }

    private void InitActionBar()
    {
        this.activeItemsBar = new ActiveItemsBar(new ActiveItemsBar.Props
        {
            onSlotMouseUp = this.OnSlotMouseUp,
            onSlotMouseHold = this.OnSlotMouseHold,
            onInventoryButtonClicked = () => this.inventoryDrawer.ToggleShown(),
            onCraftingButtonClicked = ToggleCraftingMenu,
        });
        this.Add(this.activeItemsBar);
    }

    private void InitCraftingMenu()
    {
        this.craftingMenu = new CraftingMenu();
        this.Add(this.craftingMenu);
    }

    private void ToggleCraftingMenu()
    {
        this.craftingMenu.ToggleShown();

        if (this.inventoryDrawer.Shown != this.craftingMenu.Shown)
        {
            this.inventoryDrawer.ToggleShown();
        }
    }

    private void InitHoveringSlot()
    {
        this.hoveringSlot = new SlotItemIcon();
        this.Add(this.hoveringSlot);
        this.hoveringSlot.style.position = Position.Absolute;
        this.RegisterCallback<MouseMoveEvent>(OnMouseMoveEvent);
        this.hoveringSlot.style.display = DisplayStyle.None;
    }

    private void OnMouseMoveEvent(MouseMoveEvent e)
    {
        if (this.selectedSourceInventory != null)
        {
            this.hoveringSlot.style.top = e.localMousePosition.y;
            this.hoveringSlot.style.left = e.localMousePosition.x;
        }
    }

    private void OnSlotMouseUp(MouseUpEvent evt, Inventory inventory, int index)
    {
        if (selectedSourceInventory == null)
        {
            if (inventory.GetItemAt(index) == null)
                return;

            selectedSourceInventory = inventory;
            selectedSourceIndex = index;
            this.hoveringSlot.style.display = DisplayStyle.Flex;
            this.hoveringSlot.style.top = evt.mousePosition.y;
            this.hoveringSlot.style.left = evt.mousePosition.x;
            this.hoveringSlot.Update(inventory.GetItemAt(index));
            this.touchedByDrag = new HashSet<int>();
        }
        else
        {
            selectedSourceInventory.TransferIndex(inventory, selectedSourceIndex, index);
            selectedSourceInventory = null;
            this.hoveringSlot.style.display = DisplayStyle.None;
        }
    }

    private void OnSlotMouseHold(MouseMoveEvent evt, Inventory inventory, int index)
    {
        if (selectedSourceInventory != null)
        {
            if (!touchedByDrag.Contains(index))
            {
                Item heldItem = selectedSourceInventory.GetItemAt(selectedSourceIndex);

                selectedSourceInventory.DecrementCountOf(selectedSourceIndex, 1);
                Item split = Item.Create(heldItem.Type);
                inventory.AddItem(split, index);

                if (selectedSourceInventory.GetItemAt(selectedSourceIndex) == null)
                {
                    selectedSourceInventory = null;
                    this.hoveringSlot.style.display = DisplayStyle.None;
                }

                touchedByDrag.Add(index);
            }
        }
    }

    private void ListenForHotkeys()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            this.inventoryDrawer.ToggleShown();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCraftingMenu();
        }
    }
}