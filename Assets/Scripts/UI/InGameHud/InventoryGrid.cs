using System;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryGrid : ActiveElement
{
    private Point2Int dimensions;
    private Inventory inventory;
    private List<InventorySlot> slots = new List<InventorySlot>();
    private Action<MouseUpEvent, Inventory, int> onSlotMouseUp;
    public Action<MouseMoveEvent, Inventory, int> onSlotMouseHold;
    public int SelectedIndex = 1;

    public struct Props
    {
        public Inventory inventory;
        public Action<MouseUpEvent, Inventory, int> onSlotMouseUp;
        public Action<MouseMoveEvent, Inventory, int> onSlotMouseHold;
        public float Gap;
        public bool HideBorder;
    }

    public InventoryGrid(Props props)
    {
        onSlotMouseUp = props.onSlotMouseUp;
        onSlotMouseHold = props.onSlotMouseHold;
        inventory = props.inventory;
        dimensions = new Point2Int(props.inventory.Width, props.inventory.Height);

        BuildGrid(props);
    }

    private void BuildGrid(Props props)
    {
        slots = new List<InventorySlot>(inventory.Size);
        for (int y = 0; y < dimensions.y; y++)
        {
            var row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;
            Content.Add(row);
            for (int x = 0; x < dimensions.x; x++)
            {
                InventorySlot slot = new InventorySlot(new InventorySlot.Props
                {
                    pos = new Point2Int(x, y),
                    parentDimensions = dimensions,
                    inventory = inventory,
                    onMouseUp = onSlotMouseUp,
                    onMouseHold = onSlotMouseHold,
                    SelfSufficientBorder = props.Gap > 0,
                });

                if (x != dimensions.x - 1)
                {
                    slot.style.marginRight = props.Gap;
                }

                row.Add(slot);
                slots.Add(slot);
            }

            if (y != dimensions.y - 1)
            {
                row.style.marginBottom = props.Gap;
            }
        }
    }

    public override void Update()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].Update(inventory.GetItemAt(i));
        }
    }
}