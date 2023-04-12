using UnityEngine;
using UnityEngine.UIElements;

public class InventorySlot : VisualElement
{
    public InventorySlot()
    {
        this.style.width = 50;
        this.style.height = 50;
        this.style.backgroundColor = Color.blue;
    }
}