using System;
using Core;
using UnityEngine;
using UnityEngine.UIElements;

public class InventorySlot : VisualElement
{
    public const float Size = 75;
    private int index;
    private Inventory containingInventory;
    private Action<MouseUpEvent, Inventory, int> onMouseUp;
    public Action<MouseMoveEvent, Inventory, int> onMouseHold;
    private SlotItemIcon itemIcon;
    private VisualElement Content;

    public class Props
    {
        public Point2Int pos;
        public Point2Int parentDimensions;
        public Inventory inventory;
        public Action<MouseUpEvent, Inventory, int> onMouseUp;
        public Action<MouseMoveEvent, Inventory, int> onMouseHold;
        public bool SelfSufficientBorder;
        public float BorderWidth = 1;
    }

    public InventorySlot(Props props)
    {
        this.onMouseUp = props.onMouseUp;
        this.onMouseHold = props.onMouseHold;
        this.containingInventory = props.inventory;
        this.index = props.pos.x + props.pos.y * props.parentDimensions.x;

        this.Content = new VisualElement();
        this.Content.style.width = Size;
        this.Content.style.height = Size;
        this.Content.style.backgroundImage = new StyleBackground(UIElements.GetElement(UIElementType.Vignette));
        this.Add(this.Content);
        InitBorder(props);

        this.RegisterCallback<MouseUpEvent>(OnMouseUp);
        this.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        InitSlotIcon();
    }

    private void InitBorder(Props props)
    {
        if (props.SelfSufficientBorder)
        {
            var outerBorder = new VisualElement();
            outerBorder.SetAllBorderColor(UIManager.ColorTheme.PanelOutlineColorMid);
            outerBorder.SetAllBorderWidth(props.BorderWidth);
            outerBorder.SetAllBorderRadius(5);
            this.Add(outerBorder);

            this.Content = new VisualElement();
            this.Content.SetAllBorderRadius(5);
            outerBorder.Add(this.Content);
        }

        this.Content.SetAllBorderColor(UIManager.ColorTheme.PanelOutlineColorMid);
        this.Content.style.borderTopWidth = props.BorderWidth;
        this.Content.style.borderLeftWidth = props.BorderWidth;

        if (props.pos.x == props.parentDimensions.x - 1)
        {
            this.Content.style.borderRightWidth = props.BorderWidth;
        }

        if (props.pos.y == props.parentDimensions.y - 1)
        {
            this.Content.style.borderBottomWidth = props.BorderWidth;
        }

        if (props.pos.x == 0 && props.pos.y == 0)
        {
            this.Content.style.borderTopLeftRadius = 10;
        }

        if (props.pos.x == props.parentDimensions.x - 1 && props.pos.y == 0)
        {
            this.Content.style.borderTopRightRadius = 10;
        }

        if (props.pos.x == 0 && props.pos.y == props.parentDimensions.y - 1)
        {
            this.Content.style.borderBottomLeftRadius = 10;
        }

        if (props.pos.x == props.parentDimensions.x - 1 && props.pos.y == props.parentDimensions.y - 1)
        {
            this.Content.style.borderBottomRightRadius = 10;
        }

    }

    private void InitSlotIcon()
    {
        this.itemIcon = new SlotItemIcon();
        this.Content.Add(this.itemIcon);
        this.Content.style.alignItems = Align.Center;
        this.Content.style.justifyContent = Justify.Center;
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

    public void Update(Item item)
    {
        this.itemIcon.Update(item);

        if (PlayerMono.Instance.SelectedInventory == this.containingInventory &&
            PlayerMono.Instance.SelectedInventoryIndex == this.index)
        {
            this.Content.style.backgroundColor = UIManager.ColorTheme.SelectedInventorySlot;
        }
        else if (item != null)
        {
            this.Content.style.backgroundColor = UIManager.ColorTheme.OccupiedInventorySlot;
        }
    }
}