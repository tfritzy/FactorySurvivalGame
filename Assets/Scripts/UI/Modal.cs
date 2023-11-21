using System;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Modal : ActiveElement
{
    protected VisualElement modal;

    public Modal(Action? onClose = null)
    {
        this.style.position = Position.Absolute;
        this.style.left = 0;
        this.style.top = 0;
        this.style.width = Length.Percent(100);
        this.style.height = Length.Percent(100);
        this.style.alignItems = Align.Center;
        this.style.justifyContent = Justify.Center;
        this.pickingMode = PickingMode.Ignore;

        this.modal = new VisualElement();
        this.modal.style.backgroundColor = UIManager.ColorTheme.PanelBackgroundColor;
        UIManager.ColorTheme.Apply3DPanelBorderColor(this.modal);
        this.modal.SetAllBorderRadius(10);
        this.modal.SetAllBorderWidth(1);
        this.modal.SetAllPadding(10);
        this.Add(this.modal);

        if (onClose != null)
        {
            var closeButton = new Button();
            closeButton.style.position = Position.Absolute;
            closeButton.style.right = -1;
            closeButton.style.top = 0;
            closeButton.style.backgroundColor = Color.red;
            closeButton.style.width = 40;
            closeButton.style.height = 40;
            closeButton.SetAllBorderRadius(10);
            closeButton.SetAllBorderWidth(0);
            closeButton.clicked += onClose;
            modal.Add(closeButton);
        }
    }
}