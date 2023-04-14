using System;
using Core;
using UnityEngine;
using UnityEngine.UIElements;

public class InventorySlot : ActiveElement
{
    private const float borderWidth = 2;
    private const float size = 50;
    private int index;
    private InventoryComponent containingInventory;
    private Action<InventoryComponent, int> onSelect;

    public struct Props
    {
        public Point2Int pos;
        public Point2Int parentDimensions;
        public InventoryComponent inventory;
        public Action<InventoryComponent, int> onSelect;
    }

    public InventorySlot(Props props)
    {
        this.onSelect = props.onSelect;
        this.containingInventory = props.inventory;
        this.index = props.pos.x + props.pos.y * props.parentDimensions.x;

        this.style.backgroundColor = Color.blue;
        this.style.width = size;
        this.style.height = size;

        FormatBorder(props.pos, props.parentDimensions);
        this.RegisterCallback<MouseUpEvent>(OnMouseUp);
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

    private void OnMouseUp(MouseUpEvent evt)
    {
        this.onSelect(this.containingInventory, this.index);
    }

    public override void Update()
    {
        if (this.containingInventory.GetItemAt(this.index) != null)
        {
            this.style.backgroundColor = Color.green;
        }
        else
        {
            this.style.backgroundColor = Color.blue;
        }
    }
}