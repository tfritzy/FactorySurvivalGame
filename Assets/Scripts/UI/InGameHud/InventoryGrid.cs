using System;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryGrid : ActiveElement
{
    private Point2Int dimensions;
    private InventoryComponent inventory;
    private List<InventorySlot> slots = new List<InventorySlot>();
    private Action<MouseUpEvent, InventoryComponent, int> onSlotMouseUp;
    public Action<MouseMoveEvent, InventoryComponent, int> onSlotMouseHold;

    public struct Props
    {
        public int width;
        public int height;
        public InventoryComponent inventory;
        public Action<MouseUpEvent, InventoryComponent, int> onSlotMouseUp;
        public Action<MouseMoveEvent, InventoryComponent, int> onSlotMouseHold;
        public float Gap;
        public bool HideBorder;
        public float? SlotBorderWidth;
    }

    public InventoryGrid(Props props)
    {
        this.onSlotMouseUp = props.onSlotMouseUp;
        this.onSlotMouseHold = props.onSlotMouseHold;
        this.inventory = props.inventory;
        this.dimensions = new Point2Int(props.width, props.height);

        InitBorder(props);
        BuildGrid(props);
    }

    private void InitBorder(Props props)
    {
        if (!props.HideBorder)
        {
            var outerBorder = new VisualElement();
            UI.ColorTheme.Apply3DPanelBorderColor(outerBorder, inverse: true);
            outerBorder.SetAllBorderWidth(2);
            outerBorder.SetAllBorderRadius(10);
            this.Add(outerBorder);

            var innerBorder = new VisualElement();
            UI.ColorTheme.Apply3DPanelBorderColor(innerBorder);
            innerBorder.SetAllBorderWidth(2);
            innerBorder.SetAllBorderRadius(10);
            outerBorder.Add(innerBorder);

            this.Content = innerBorder;
        }
        else
        {
            this.Content = this;
        }
    }

    private void BuildGrid(Props props)
    {
        slots = new List<InventorySlot>(inventory.Size);
        for (int y = 0; y < this.dimensions.y; y++)
        {
            var row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;
            row.style.justifyContent = Justify.SpaceBetween;
            this.Content.Add(row);
            for (int x = 0; x < this.dimensions.x; x++)
            {
                InventorySlot slot = new InventorySlot(new InventorySlot.Props
                {
                    pos = new Point2Int(x, y),
                    parentDimensions = this.dimensions,
                    inventory = this.inventory,
                    onMouseUp = this.onSlotMouseUp,
                    onMouseHold = this.onSlotMouseHold,
                    SelfSufficientBorder = props.Gap > 0,
                    BorderWidth = props.SlotBorderWidth ?? 2,
                });

                if (x != this.dimensions.x - 1)
                {
                    slot.style.marginRight = props.Gap;
                }

                row.Add(slot);
                slots.Add(slot);
            }

            if (y != this.dimensions.y - 1)
            {
                row.style.marginBottom = props.Gap;
            }
        }
    }

    public override void Update()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].Update();
        }
    }
}