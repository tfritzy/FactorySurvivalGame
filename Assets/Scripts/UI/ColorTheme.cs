using UnityEngine;
using UnityEngine.UIElements;

public abstract class ColorTheme
{
    public abstract Color ItemSlotTextColor { get; }

    public abstract Color PanelBackgroundColor { get; }

    public abstract Color PanelOutlineColorBright { get; }
    public abstract Color PanelOutlineColorMid { get; }
    public abstract Color PanelOutlineColorDark { get; }
    public void ApplyPanelBorderColor(VisualElement element, bool inverse = false)
    {
        if (!inverse)
        {
            element.style.borderTopColor = UI.ColorTheme.PanelOutlineColorDark;
            element.style.borderRightColor = UI.ColorTheme.PanelOutlineColorDark;
            element.style.borderBottomColor = UI.ColorTheme.PanelOutlineColorBright;
            element.style.borderLeftColor = UI.ColorTheme.PanelOutlineColorBright;
        }
        else
        {
            element.style.borderBottomColor = UI.ColorTheme.PanelOutlineColorDark;
            element.style.borderLeftColor = UI.ColorTheme.PanelOutlineColorDark;
            element.style.borderTopColor = UI.ColorTheme.PanelOutlineColorBright;
            element.style.borderRightColor = UI.ColorTheme.PanelOutlineColorBright;
        }
    }

    public abstract Color OccupiedInventorySlot { get; }
}