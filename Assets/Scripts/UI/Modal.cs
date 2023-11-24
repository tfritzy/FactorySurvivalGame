using System;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Modal : ActiveElement
{
    protected VisualElement modal;

    public Modal(Action? onClose = null)
    {
        style.position = Position.Absolute;
        style.left = 0;
        style.top = 0;
        style.width = Length.Percent(100);
        style.height = Length.Percent(100);
        style.alignItems = Align.Center;
        style.justifyContent = Justify.Center;

        modal = new VisualElement();
        modal.style.backgroundColor = UIManager.ColorTheme.PanelBackgroundColor;
        UIManager.ColorTheme.Apply3DPanelBorderColor(modal);
        modal.SetAllBorderRadius(10);
        modal.SetAllBorderWidth(1);
        modal.SetAllPadding(10);
        Add(modal);

        if (onClose != null)
        {
            var closeButton = new Button();
            closeButton.style.position = Position.Absolute;
            closeButton.style.right = -2;
            closeButton.style.top = 0;
            closeButton.text = "✕";
            closeButton.style.fontSize = 20;
            closeButton.style.color = UIManager.ColorTheme.PrimaryText;
            closeButton.style.width = 40;
            closeButton.style.height = 40;
            closeButton.style.backgroundColor = Color.clear;
            closeButton.SetAllBorderWidth(0);
            closeButton.clicked += onClose;
            modal.Add(closeButton);
        }
    }
}