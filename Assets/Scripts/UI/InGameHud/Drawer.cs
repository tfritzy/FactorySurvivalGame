using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Drawer : ActiveElement
{
    private VisualElement OuterElement;

    public Drawer()
    {
        this.Content.style.position = Position.Absolute;
        this.Content.style.right = 0;
        this.Content.style.top = 0;
        this.Content.style.minHeight = Length.Percent(100);

        this.Content.style.transitionProperty = new List<StylePropertyName> { "translate" };
        List<EasingFunction> easingFunctions = new() { new(EasingMode.EaseOut) };
        this.Content.style.transitionTimingFunction = new StyleList<EasingFunction>(easingFunctions);
        this.Content.style.transitionDuration = new StyleList<TimeValue>(new List<TimeValue> { new(0.3f, TimeUnit.Second) });
        this.Content.style.translate = new StyleTranslate(new Translate(500, 0, 0));
        OuterElement = this.contentContainer;
        RegisterCallback<GeometryChangedEvent>((e) =>
        {
            OuterElement.style.translate = new StyleTranslate(new Translate(0, 0, 0));
        });

        var gradient = new GradientElement(
            UIManager.ColorTheme.PanelGradientStart,
            UIManager.ColorTheme.PanelGradientEnd);
        gradient.style.minHeight = Length.Percent(100);
        this.Content.Add(gradient);
        this.Content = gradient;

        var content = new VisualElement();
        content.style.minHeight = Length.Percent(100);
        this.Content.Add(content);
        this.Content = content;
        this.Content.SetAllPadding(10);
        this.Content.style.borderLeftWidth = 1;
        this.Content.style.borderLeftColor = UIManager.ColorTheme.PanelOutlineColorDark;
    }

    public override void Show()
    {
        this.OuterElement.style.translate = new StyleTranslate(new Translate(0, 0, 0));
    }

    public override void Hide()
    {
        this.OuterElement.style.translate = new StyleTranslate(new Translate(500, 0, 0));
    }
}