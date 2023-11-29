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
        style.width = Length.Percent(100);
        style.height = Length.Percent(100);
        style.alignItems = Align.Center;
        style.flexDirection = FlexDirection.ColumnReverse;

        InitInventory();
        InitActionBar();
        InitCraftingMenu();
        InitHoveringSlot();
    }

    public override void Update()
    {
        inventoryDrawer.Update();
        activeItemsBar.Update();

        ListenForHotkeys();
    }

    private void InitInventory()
    {
        Player player = PlayerMono.Instance.Actual;

        inventoryDrawer = new InventoryDrawer(new InventoryDrawer.Props
        {
            inventory = player.GetComponent<Inventory>(),
            onSlotMouseUp = OnSlotMouseUp,
            onSlotMouseHold = OnSlotMouseHold,
        });
        Add(inventoryDrawer);
    }

    private void InitActionBar()
    {
        activeItemsBar = new ActiveItemsBar(new ActiveItemsBar.Props
        {
            onSlotMouseUp = OnSlotMouseUp,
            onSlotMouseHold = OnSlotMouseHold,
            onInventoryButtonClicked = () => inventoryDrawer.ToggleShown(),
            onCraftingButtonClicked = ToggleCraftingMenu,
        });
        Add(activeItemsBar);
    }

    private void InitCraftingMenu()
    {
        craftingMenu = new CraftingMenu();
        Add(craftingMenu);
    }

    private void ToggleCraftingMenu()
    {
        craftingMenu.ToggleShown();

        if (inventoryDrawer.Shown != craftingMenu.Shown)
        {
            inventoryDrawer.ToggleShown();
        }
    }

    private void InitHoveringSlot()
    {
        hoveringSlot = new SlotItemIcon();
        Add(hoveringSlot);
        hoveringSlot.style.position = Position.Absolute;
        RegisterCallback<MouseMoveEvent>(OnMouseMoveEvent);
        hoveringSlot.style.display = DisplayStyle.None;
    }

    private void OnMouseMoveEvent(MouseMoveEvent e)
    {
        if (selectedSourceInventory != null)
        {
            hoveringSlot.style.top = e.localMousePosition.y;
            hoveringSlot.style.left = e.localMousePosition.x;
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
            hoveringSlot.style.display = DisplayStyle.Flex;
            hoveringSlot.style.top = evt.mousePosition.y;
            hoveringSlot.style.left = evt.mousePosition.x;
            hoveringSlot.Update(inventory.GetItemAt(index));
            touchedByDrag = new HashSet<int>();
        }
        else
        {
            selectedSourceInventory.TransferIndex(inventory, selectedSourceIndex, index);
            selectedSourceInventory = null;
            hoveringSlot.style.display = DisplayStyle.None;
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
                    hoveringSlot.style.display = DisplayStyle.None;
                }

                touchedByDrag.Add(index);
            }
        }
    }

    private void ListenForHotkeys()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryDrawer.ToggleShown();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCraftingMenu();
        }
    }
}