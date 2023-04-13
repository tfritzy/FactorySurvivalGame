using Core;
using UnityEngine;
using UnityEngine.UIElements;

public class InventorySlot : ActiveElement
{
    private const float borderWidth = 2;
    private const float size = 50;
    private int index;
    private InventoryComponent containingInventory;

    public InventorySlot(Point2Int pos, Point2Int parentDimensions, InventoryComponent inventory)
    {
        this.containingInventory = inventory;
        this.style.width = size;
        this.style.height = size;
        this.index = pos.x + pos.y * parentDimensions.x;
        this.style.backgroundColor = Color.blue;
        FormatBorder(pos, parentDimensions);

        this.RegisterCallback<MouseUpEvent>(OnMouseUp);
    }

    private void OnMouseUp(MouseUpEvent evt)
    {
        if (evt.button == 0)
        {
            this.style.color = Color.red;
        }
        else if (evt.button == 1)
        {
            this.style.color = Color.green;
        }
    }

    private void FormatBorder(Point2Int pos, Point2Int dimensions)
    {
        this.SetAllBorderWidth(borderWidth);
        this.SetAllBorderColor(Color.black);

        if (pos.x == 0)
        {
            this.style.borderLeftWidth = borderWidth * 2;
        }

        if (pos.y == 0)
        {
            this.style.borderTopWidth = borderWidth * 2;
        }

        if (pos.x == dimensions.x - 1)
        {
            this.style.borderRightWidth = borderWidth * 2;
        }

        if (pos.y == dimensions.y - 1)
        {
            this.style.borderBottomWidth = borderWidth * 2;
        }
    }

    public override void Update()
    {
        if (this.containingInventory.GetItem(this.index) != null)
        {
            this.style.backgroundColor = Color.green;
        }
        else
        {
            this.style.backgroundColor = Color.red;
        }
    }
}