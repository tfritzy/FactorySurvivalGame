using UnityEngine;
using UnityEngine.UIElements;

public static class UIHelpers
{
    public static void SetAllBorderColor(this VisualElement element, Color color)
    {
        element.style.borderLeftColor = color;
        element.style.borderRightColor = color;
        element.style.borderTopColor = color;
        element.style.borderBottomColor = color;
    }

    public static void SetAllBorderWidth(this VisualElement element, float width)
    {
        element.style.borderLeftWidth = width;
        element.style.borderRightWidth = width;
        element.style.borderTopWidth = width;
        element.style.borderBottomWidth = width;
    }

    public static void SetAllPadding(this VisualElement element, float padding)
    {
        element.style.paddingLeft = padding;
        element.style.paddingRight = padding;
        element.style.paddingTop = padding;
        element.style.paddingBottom = padding;
    }

    public static void SetAllMargin(this VisualElement element, float margin)
    {
        element.style.marginLeft = margin;
        element.style.marginRight = margin;
        element.style.marginTop = margin;
        element.style.marginBottom = margin;
    }
}