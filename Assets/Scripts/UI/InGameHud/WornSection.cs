using Core;
using UnityEngine;
using UnityEngine.UIElements;

public class WornItemsSection : ActiveElement
{
    public WornItemsSection()
    {
        this.style.alignItems = Align.Center;
        this.style.justifyContent = Justify.Center;

        this.Content = new VisualElement();
        this.Content.SetAllBorderColor(UI.ColorTheme.PanelOutlineColorBright);
        this.Content.SetAllBorderWidth(2);
        this.Content.SetAllBorderRadius(5);
        this.Content.style.flexDirection = FlexDirection.Row;
        this.Add(this.Content);

        InitItemsSection();
        InitDivider();
        InitPortrait();
    }

    private void InitItemsSection()
    {
        var dummy = new Dummy(Managers.World.Context, 0);
        Inventory inventory = new Inventory(dummy, 5, 1);
        var grid = new InventoryGrid(new InventoryGrid.Props
        {
            height = 5,
            width = 1,
            inventory = inventory,
            Gap = 10,
            HideBorder = true,
            SlotBorderWidth = 1,
        });

        var gridContainer = new VisualElement();
        gridContainer.SetAllPadding(10);
        gridContainer.Add(grid);
        this.Content.Add(gridContainer);
    }

    private void InitDivider()
    {
        var divider = new VisualElement();
        divider.style.width = 2;
        divider.style.height = Length.Percent(100);
        divider.style.backgroundColor = UI.ColorTheme.PanelOutlineColorBright;
        this.Content.Add(divider);
    }

    private void InitPortrait()
    {
        var portraitBox = new VisualElement();
        portraitBox.style.width = 250;
        portraitBox.style.height = Length.Percent(100);
        portraitBox.style.alignItems = Align.Center;
        portraitBox.style.justifyContent = Justify.Center;
        portraitBox.style.backgroundColor = UI.ColorTheme.PanelForegroundColor;

        this.Content.Add(portraitBox);
    }

    public override void Update()
    {
    }
}