using UnityEngine;
using UnityEngine.UIElements;

public abstract class ColorTheme
{
    public abstract Color PrimaryText { get; }
    public abstract Color ItemSlotTextColor { get; }

    public abstract Color PanelBackgroundColor { get; }
    public abstract Color PanelForegroundColor { get; }

    public abstract Color PanelOutlineColorBright { get; }
    public abstract Color PanelOutlineColorMid { get; }
    public abstract Color PanelOutlineColorDark { get; }
    public void Apply3DPanelBorderColor(VisualElement element, bool inverse = false)
    {
        if (!inverse)
        {
            element.style.borderTopColor = UIManager.ColorTheme.PanelOutlineColorDark;
            element.style.borderRightColor = UIManager.ColorTheme.PanelOutlineColorDark;
            element.style.borderBottomColor = UIManager.ColorTheme.PanelOutlineColorBright;
            element.style.borderLeftColor = UIManager.ColorTheme.PanelOutlineColorBright;
        }
        else
        {
            element.style.borderBottomColor = UIManager.ColorTheme.PanelOutlineColorDark;
            element.style.borderLeftColor = UIManager.ColorTheme.PanelOutlineColorDark;
            element.style.borderTopColor = UIManager.ColorTheme.PanelOutlineColorBright;
            element.style.borderRightColor = UIManager.ColorTheme.PanelOutlineColorBright;
        }
    }

    public abstract Color OccupiedInventorySlot { get; }
    public abstract Color SelectedInventorySlot { get; }
    public abstract Color PanelGradientStart { get; }
    public abstract Color PanelGradientEnd { get; }
}