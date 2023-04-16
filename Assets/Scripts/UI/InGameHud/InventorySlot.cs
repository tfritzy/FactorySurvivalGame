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

        this.style.width = Size;
        this.style.height = Size;
        this.style.backgroundImage = new StyleBackground(UIElements.GetElement(UIElementType.Vignette));
        this.style.backgroundColor = Color.black;
        InitBorder(props.pos, props.parentDimensions);

        this.RegisterCallback<MouseUpEvent>(OnMouseUp);
        this.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        InitSlotIcon();
    }

    private void InitBorder(Point2Int pos, Point2Int dimensions)
    {
        this.SetAllBorderWidth(borderWidth);
        UI.ColorTheme.ApplyPanelBorderColor(this);

        if (pos.x == 0 && pos.y == 0)
        {
            this.style.borderTopLeftRadius = 5;
        }

        if (pos.x == dimensions.x - 1 && pos.y == 0)
        {
            this.style.borderTopLeftRadius = 5;
        }

        if (pos.x == 0 && pos.y == dimensions.y - 1)
        {
            this.style.borderBottomLeftRadius = 5;
        }

        if (pos.x == dimensions.x - 1 && pos.y == dimensions.y - 1)
        {
            this.style.borderBottomRightRadius = 5;
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
            this.style.backgroundColor = UI.ColorTheme.OccupiedInventorySlot;
        }
        else
        {
            this.style.backgroundColor = UI.ColorTheme.PanelBackgroundColor;
        }
    }
}