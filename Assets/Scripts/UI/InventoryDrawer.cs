using UnityEngine;
using UnityEngine.UIElements;

public class InventoryDrawer : Drawer
{
    public InventoryDrawer()
    {
        this.style.justifyContent = Justify.SpaceBetween;

        InitWornSection();
        InitBackpackGrid();
    }

    private void InitWornSection()
    {
        var wornSection = new WornItemsSection();
        wornSection.style.height = Length.Percent(30);
        this.Add(wornSection);
    }

    private void InitBackpackGrid()
    {
        var backpackSection = new VisualElement();
        backpackSection.style.height = Length.Percent(60);
        backpackSection.Add(BuildButtonRow());
        backpackSection.Add(new InventoryGrid(10, 20));
        this.Add(backpackSection);
    }

    private VisualElement BuildButtonRow()
    {
        var buttonRow = new VisualElement();
        var dummyButton = new Button();
        dummyButton.text = "Dummy";
        buttonRow.Add(dummyButton);
        buttonRow.style.flexDirection = FlexDirection.Row;
        return buttonRow;
    }
}