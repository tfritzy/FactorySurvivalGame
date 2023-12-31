using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Modal : ActiveElement
{
    protected VisualElement modal;
    private VisualElement OutermostModal;

    public Modal(Action? onClose = null)
    {
        pickingMode = PickingMode.Ignore;
        style.position = Position.Absolute;
        style.left = 0;
        style.top = 0;
        style.width = Length.Percent(100);
        style.height = Length.Percent(100);
        style.alignItems = Align.Center;
        style.justifyContent = Justify.Center;

        modal = new VisualElement();
        modal.SetAllBorderRadius(15);
        modal.style.overflow = Overflow.Hidden;
        modal.style.backgroundColor = UIManager.ColorTheme.PanelBackgroundColor;
        modal.SetAllBorderColor(UIManager.ColorTheme.PanelOutline);
        modal.SetAllBorderWidth(1);

        Add(modal);

        modal.style.transitionProperty = new List<StylePropertyName> { "opacity", "translate" };
        List<EasingFunction> easingFunctions = new()
        {
            new(EasingMode.EaseOut),
            new(EasingMode.EaseOut)
        };
        modal.style.transitionTimingFunction = new StyleList<EasingFunction>(easingFunctions);

        List<TimeValue> delays = new()
        {
            new(0.1f, TimeUnit.Second),
            new(0.2f, TimeUnit.Second)
        };
        modal.style.transitionDuration = new StyleList<TimeValue>(delays);

        modal.style.opacity = 0;
        modal.style.translate = new StyleTranslate(new Translate(0, 25, 0));
        OutermostModal = modal;

        var content = new VisualElement();
        modal.Add(content);
        content.SetAllPadding(20);
        content.style.paddingBottom = 0;
        modal = content;

        if (onClose != null)
        {
            var closeButton = new Button();
            closeButton.style.position = Position.Absolute;
            closeButton.style.right = 0;
            closeButton.style.top = 2;
            closeButton.text = "✕";
            closeButton.style.fontSize = 26;
            closeButton.style.color = UIManager.ColorTheme.PrimaryText;
            closeButton.style.width = 40;
            closeButton.style.height = 40;
            closeButton.style.backgroundColor = Color.clear;
            closeButton.SetAllBorderWidth(0);
            closeButton.clicked += onClose;
            modal.Add(closeButton);
        }

        InputManager.Instance.RegisterKeyDown(KeyCode.Escape, Hide, -1);

        Hide();
        Shown = false;
    }

    public override void Hide()
    {
        base.Hide();
        this.style.display = DisplayStyle.None;
        OutermostModal.style.opacity = 0f;
        OutermostModal.style.translate = new StyleTranslate(new Translate(0, 25, 0));
    }

    public override void Show()
    {
        base.Show();
        this.style.display = DisplayStyle.Flex;
        OutermostModal.style.opacity = 1f;
        OutermostModal.style.translate = new StyleTranslate(new Translate(0, 0, 0));
    }
}