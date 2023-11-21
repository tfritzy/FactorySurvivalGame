using System;
using Core;
using UnityEngine;
using UnityEngine.UIElements;

public class InventorySlot : ActiveElement
{
    public const float Size = 50;
    private int index;
    private Inventory containingInventory;
    private Action<MouseUpEvent, Inventory, int> onMouseUp;
    public Action<MouseMoveEvent, Inventory, int> onMouseHold;
    private SlotItemIcon itemIcon;

    public class Props
    {
        public Point2Int pos;
        public Point2Int parentDimensions;
        public Inventory inventory;
        public Action<MouseUpEvent, Inventory, int> onMouseUp;
        public Action<MouseMoveEvent, Inventory, int> onMouseHold;
        public bool SelfSufficientBorder;
        public float BorderWidth = 2;
    }

    public InventorySlot(Props props)
    {
        this.onMouseUp = props.onMouseUp;
        this.onMouseHold = props.onMouseHold;
        this.containingInventory = props.inventory;
        this.index = props.pos.x + props.pos.y * props.parentDimensions.x;

        InitBorder(props);
        this.Content.style.width = Size;
        this.Content.style.height = Size;
        this.Content.style.backgroundImage = new StyleBackground(UIElements.GetElement(UIElementType.Vignette));

        this.RegisterCallback<MouseUpEvent>(OnMouseUp);
        this.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        InitSlotIcon();
    }

    private void InitBorder(Props props)
    {
        if (props.SelfSufficientBorder)
        {
            var outerBorder = new VisualElement();
            UIManager.ColorTheme.Apply3DPanelBorderColor(outerBorder, inverse: true);
            outerBorder.SetAllBorderWidth(props.BorderWidth);
            outerBorder.SetAllBorderRadius(5);
            this.Add(outerBorder);

            this.Content = new VisualElement();
            this.Content.SetAllBorderRadius(5);
            outerBorder.Add(this.Content);
        }

        this.Content.SetAllBorderWidth(props.BorderWidth);
        UIManager.ColorTheme.Apply3DPanelBorderColor(this.Content);

        if (props.pos.x == 0 && props.pos.y == 0)
        {
            this.Content.style.borderTopLeftRadius = 5;
        }

        if (props.pos.x == props.parentDimensions.x - 1 && props.pos.y == 0)
        {
            this.Content.style.borderTopLeftRadius = 5;
        }

        if (props.pos.x == 0 && props.pos.y == props.parentDimensions.y - 1)
        {
            this.Content.style.borderBottomLeftRadius = 5;
        }

        if (props.pos.x == props.parentDimensions.x - 1 && props.pos.y == props.parentDimensions.y - 1)
        {
            this.Content.style.borderBottomRightRadius = 5;
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

    public override void Update()
    {
        var item = this.containingInventory.GetItemAt(this.index);
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
        else
        {
            this.Content.style.backgroundColor = UIManager.ColorTheme.PanelBackgroundColor;
        }
    }
}