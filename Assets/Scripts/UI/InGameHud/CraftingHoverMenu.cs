using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.UIElements;

public class CraftingHoverMenu : VisualElement
{
    private Dictionary<ItemType, Label> quantityLabels = new();
    private Item item;

    public CraftingHoverMenu(Item item, float x, float y)
    {
        this.item = item;
        style.flexDirection = FlexDirection.Column;
        style.position = Position.Absolute;
        style.left = x + 10;
        style.top = y;
        style.width = 200;
        style.backgroundColor = ColorTheme.Current.PanelBackgroundColor;
        this.SetAllBorderRadius(10);
        this.SetAllPadding(5);
        this.SetAllBorderWidth(1);
        this.SetAllBorderColor(ColorTheme.Current.PanelOutline);

        var label = new Label(item.Name);
        label.style.fontSize = 16;
        label.style.color = ColorTheme.Current.PrimaryText;
        Add(label);

        if (item.Recipe != null)
        {
            foreach (var type in item.Recipe.Keys)
            {
                var quantityHeld =
                    PlayerMono.Instance.Actual.Inventory.GetItemCount(type) +
                    PlayerMono.Instance.Actual.ActiveItems.GetItemCount(type);
                var recipeItem = Item.Create(type);
                var quantity = item.Recipe[type];

                var recipeEl = new VisualElement();
                recipeEl.style.flexDirection = FlexDirection.Row;
                recipeEl.style.alignItems = Align.Center;
                var icon = new UiItem();
                icon.Update(recipeItem);
                recipeEl.Add(icon);

                var amountNeeded = new Label();
                amountNeeded.style.marginLeft = 5;
                amountNeeded.style.fontSize = 16;
                quantityLabels[type] = amountNeeded;

                recipeEl.Add(amountNeeded);
                Add(recipeEl);
            }

            UpdateQuantityLabels();
        }
    }

    private void UpdateQuantityLabels()
    {
        if (item.Recipe == null)
            return;

        foreach (var type in quantityLabels.Keys)
        {
            var quantityHeld =
                PlayerMono.Instance.Actual.Inventory.GetItemCount(type) +
                PlayerMono.Instance.Actual.ActiveItems.GetItemCount(type);
            var quantity = item.Recipe[type];
            var amountNeeded = quantityLabels[type];

            var heldString = quantityHeld == 0 ? "-" : quantityHeld.ToString();
            amountNeeded.text = $"{heldString}/{quantity} x {Item.Create(type).Name}";

            if (quantityHeld < quantity)
                amountNeeded.style.color = Color.red;
            else
                amountNeeded.style.color = ColorTheme.Current.PrimaryText;
        }
    }

    public void Update()
    {
        UpdateQuantityLabels();
    }
}