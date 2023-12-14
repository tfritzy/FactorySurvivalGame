using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemSearch : VisualElement
{
    private Dictionary<Item, VisualElement> itemElements = new Dictionary<Item, VisualElement>();
    private const int itemWidth = 100;

    public ItemSearch(List<Item> items, Action<Item, VisualElement> onHover, Action onExit, Action<Item> onClick)
    {
        style.width = itemWidth * 4;
        var input = new TextField();
        input.SetAllMargin(0);
        input.style.marginBottom = 15;
        input.style.backgroundColor = Color.clear;
        input.textEdition.placeholder = "Search...";
        var textInput = input.Children().First();
        textInput.SetAllBorderRadius(20);
        textInput.SetAllPadding(2);
        textInput.SetAllBorderColor(Color.black);
        textInput.style.backgroundColor = UIManager.ColorTheme.PanelBackgroundColor;
        textInput.style.paddingLeft = 10;
        var label = textInput.Children().First();
        label.style.color = UIManager.ColorTheme.PrimaryText;
        label.SetAllMargin(7);
        label.style.fontSize = 16;

        input.SetAllBorderRadius(20);
        input.RegisterValueChangedCallback((evt) =>
        {
            var value = evt.newValue;
            foreach (var item in itemElements.Keys)
            {
                var itemEl = itemElements[item];
                if (item.Name.ToLower().Contains(value.ToLower()))
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
            textInput.style.backgroundColor = UIManager.ColorTheme.PanelBackgroundColor;
            InputManager.Instance.SetEnabled(false);
        });
        input.RegisterCallback<FocusOutEvent>((evt) =>
        {
            textInput.style.backgroundColor = UIManager.ColorTheme.PanelForegroundColor;
            InputManager.Instance.SetEnabled(true);
        });
        Add(input);

        var scrollView = new ScrollView();
        scrollView.verticalScroller.visible = false;
        scrollView.verticalScroller.style.width = 0;
        scrollView.mouseWheelScrollSize = 100;
        Add(scrollView);

        var itemContainer = new VisualElement();
        itemContainer.style.flexDirection = FlexDirection.Row;
        itemContainer.style.flexWrap = Wrap.Wrap;
        foreach (Item item in items)
        {
            var itemWithName = new VisualElement();
            Item itemCopy = item;
            itemWithName.RegisterCallback<MouseEnterEvent>((evt) =>
            {
                onHover(itemCopy, itemWithName);
            });
            itemWithName.RegisterCallback<MouseLeaveEvent>((evt) =>
            {
                onExit();
            });
            itemWithName.RegisterCallback<MouseDownEvent>((evt) =>
            {
                onClick(itemCopy);
            });

            var uiItem = new UiItem(withLabel: true);
            uiItem.Update(item);
            itemWithName.Add(uiItem);

            var itemName = new Label();
            itemName.text = item.Name.ToString();
            itemName.style.color = UIManager.ColorTheme.PrimaryText;
            itemName.style.unityTextAlign = TextAnchor.MiddleCenter;
            itemWithName.Add(itemName);

            itemWithName.style.flexDirection = FlexDirection.Column;
            itemWithName.style.alignItems = Align.Center;
            itemWithName.style.width = itemWidth;
            itemWithName.style.paddingBottom = 10;
            itemWithName.style.paddingTop = 10;

            itemContainer.Add(itemWithName);
            this.itemElements.Add(item, itemWithName);
        }
        scrollView.contentContainer.Add(itemContainer);
    }
}