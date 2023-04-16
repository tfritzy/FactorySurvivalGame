using Core;
using UnityEngine;
using UnityEngine.UIElements;

public class CraftingItemSlot : VisualElement
{
    private Item item;

    public CraftingItemSlot(ItemType itemType)
    {
        this.item = Item.Create(itemType);

        this.style.width = InventorySlot.Size;
        this.style.height = InventorySlot.Size;
        this.style.backgroundColor = Color.blue;
        this.style.alignItems = Align.Center;
        this.style.justifyContent = Justify.Center;

        var slotIcon = new SlotItemIcon();
        slotIcon.Update(this.item);
        this.Add(new SlotItemIcon());
    }
}