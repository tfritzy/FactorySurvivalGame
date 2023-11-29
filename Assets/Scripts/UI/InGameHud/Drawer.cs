using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Drawer : ActiveElement
{
    private VisualElement OuterElement;

    public Drawer()
    {
        Content.style.position = Position.Absolute;
        Content.style.right = 0;
        Content.style.top = 0;
        Content.style.minHeight = Length.Percent(100);

        Content.style.transitionProperty = new List<StylePropertyName> { "translate" };
        List<EasingFunction> easingFunctions = new() { new(EasingMode.EaseOut) };
        Content.style.transitionTimingFunction = new StyleList<EasingFunction>(easingFunctions);
        Content.style.transitionDuration = new StyleList<TimeValue>(new List<TimeValue> { new(0.3f, TimeUnit.Second) });
        Content.style.translate = new StyleTranslate(new Translate(500, 0, 0));
        OuterElement = contentContainer;

        var gradient = new GradientElement(
            UIManager.ColorTheme.PanelGradientStart,
            UIManager.ColorTheme.PanelGradientEnd);
        gradient.style.minHeight = Length.Percent(100);
        Content.Add(gradient);
        Content = gradient;

        var content = new VisualElement();
        content.style.minHeight = Length.Percent(100);
        Content.Add(content);
        Content = content;
        Content.SetAllPadding(10);
        Content.style.borderLeftWidth = 1;
        Content.style.borderLeftColor = UIManager.ColorTheme.PanelOutlineColorDark;

        Shown = false;
    }

    public override void Show()
    {
        OuterElement.style.translate = new StyleTranslate(new Translate(0, 0, 0));
    }

    public override void Hide()
    {
        OuterElement.style.translate = new StyleTranslate(new Translate(500, 0, 0));
    }
}