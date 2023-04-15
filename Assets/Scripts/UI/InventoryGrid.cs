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
    }

    public InventoryGrid(Props props)
    {
        this.onSlotMouseUp = props.onSlotMouseUp;
        this.onSlotMouseHold = props.onSlotMouseHold;
        this.inventory = props.inventory;
        this.dimensions = new Point2Int(props.width, props.height);

        this.style.height = Length.Percent(100);
        this.style.width = Length.Percent(100);
        this.style.backgroundColor = Color.yellow;

        BuildGrid();
    }

    private void BuildGrid()
    {
        slots = new List<InventorySlot>(inventory.Size);
        for (int y = 0; y < this.dimensions.y; y++)
        {
            var row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;
            this.Add(row);
            for (int x = 0; x < this.dimensions.x; x++)
            {
                InventorySlot slot = new InventorySlot(new InventorySlot.Props
                {
                    pos = new Point2Int(x, y),
                    parentDimensions = this.dimensions,
                    inventory = this.inventory,
                    onMouseUp = this.onSlotMouseUp,
                    onMouseHold = this.onSlotMouseHold
                });
                row.Add(slot);
                slots.Add(slot);
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