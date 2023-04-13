using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryGrid : ActiveElement
{
    private Point2Int dimensions;
    private InventoryComponent inventory;
    private List<InventorySlot> slots = new List<InventorySlot>();

    public InventoryGrid(int width, int height, InventoryComponent inventory)
    {
        this.inventory = inventory;

        this.dimensions = new Point2Int(width, height);

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
                InventorySlot slot = new InventorySlot(new Point2Int(x, y), this.dimensions, inventory);
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