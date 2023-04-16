using System;
using Core;
using UnityEngine;
using UnityEngine.UIElements;

public class InventorySlot : ActiveElement
{
    public const float Size = 50;
    private const float borderWidth = 2;
    private int index;
    private InventoryComponent containingInventory;
    private Action<MouseUpEvent, InventoryComponent, int> onMouseUp;
    public Action<MouseMoveEvent, InventoryComponent, int> onMouseHold;
    private SlotItemIcon itemIcon;

    public struct Props
    {
        public Point2Int pos;
        public Point2Int parentDimensions;
        public InventoryComponent inventory;
        public Action<MouseUpEvent, InventoryComponent, int> onMouseUp;
        public Action<MouseMoveEvent, InventoryComponent, int> onMouseHold;
    }

    public InventorySlot(Props props)
    {
        this.onMouseUp = props.onMouseUp;
        this.onMouseHold = props.onMouseHold;
        this.containingInventory = props.inventory;
        this.index = props.pos.x + props.pos.y * props.parentDimensions.x;

        this.style.backgroundColor = Color.blue;
        this.style.width = Size;
        this.style.height = Size;

        InitBorder(props.pos, props.parentDimensions);
        this.RegisterCallback<MouseUpEvent>(OnMouseUp);
        this.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        InitSlotIcon();
    }

    private void InitBorder(Point2Int pos, Point2Int dimensions)
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

    private void InitSlotIcon()
    {
        this.itemIcon = new SlotItemIcon();
        this.Add(this.itemIcon);
        this.style.alignItems = Align.Center;
        this.style.justifyContent = Justify.Center;
    }

    private void OnMouseUp(MouseUpEvent evt)
    {
        this.onMouseUp(evt, this.containingInventory, this.index);
    }

    private void OnMouseMove(MouseMoveEvent evt)
    {
        if (evt.pressedButtons > 0)
        {
            this.onMouseHold(evt, this.containingInventory, this.index);
        }
    }

    public override void Update()
    {
        var item = this.containingInventory.GetItemAt(this.index);
        this.itemIcon.Update(item);

        if (item != null)
        {
            this.style.backgroundColor = Color.green;
        }
        else
        {
            this.style.backgroundColor = Color.blue;
        }
    }
}