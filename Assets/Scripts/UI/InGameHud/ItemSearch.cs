using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemSearch : VisualElement
{
    private List<UiItem> itemElements = new List<UiItem>();

    public ItemSearch(List<Item> items)
    {
        style.width = 400;
        var input = new TextField();
        input.style.backgroundColor = Color.clear;
        var textInput = input.Children().First();
        textInput.SetAllBorderRadius(10);
        textInput.SetAllPadding(5);
        textInput.style.backgroundColor = Color.clear;
        textInput.style.paddingLeft = 5;
        var label = textInput.Children().First();
        label.style.color = UIManager.ColorTheme.PrimaryText;
        label.style.marginLeft = 5;

        input.SetAllBorderRadius(10);
        input.RegisterValueChangedCallback((evt) =>
        {
            var value = evt.newValue;
            foreach (var itemEl in itemElements)
            {
                if (itemEl.Item.Type.ToString().ToLower().Contains(value.ToLower()))
                {
                    itemEl.style.display = DisplayStyle.Flex;
                }
                else
                {
                    itemEl.style.display = DisplayStyle.None;
                }
            }
        });
        input.RegisterCallback<FocusInEvent>((evt) =>
        {
            InputManager.Instance.SetEnabled(false);
        });
        input.RegisterCallback<FocusOutEvent>((evt) =>
        {
            InputManager.Instance.SetEnabled(true);
        });
        Add(input);

        var itemContainer = new VisualElement();
        itemContainer.style.flexDirection = FlexDirection.Row;
        itemContainer.style.flexWrap = Wrap.Wrap;
        foreach (var item in items)
        {
            var uiItem = new UiItem();
            uiItem.style.backgroundColor = UIManager.ColorTheme.PanelForegroundColor;
            uiItem.SetAllBorderRadius(10);
            uiItem.style.marginRight = 10;
            uiItem.Update(item);
            this.itemElements.Add(uiItem);
            itemContainer.Add(uiItem);
        }
        Add(itemContainer);
    }
}