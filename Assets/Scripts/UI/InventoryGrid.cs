using UnityEngine;
using UnityEngine.UIElements;

public class InventoryGrid : VisualElement
{
    private int width;
    private int height;

    public InventoryGrid(int width, int height)
    {
        this.width = width;
        this.height = height;

        this.style.height = Length.Percent(100);
        this.style.width = Length.Percent(100);
        this.style.backgroundColor = Color.yellow;

        BuildGrid();
    }

    private void BuildGrid()
    {
        for (int y = 0; y < height; y++)
        {
            var row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;
            this.Add(row);
            for (int x = 0; x < width; x++)
            {
                row.Add(new InventorySlot());
            }
        }
    }
}