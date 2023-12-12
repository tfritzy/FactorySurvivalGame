using UnityEngine;
using UnityEngine.UIElements;

public abstract class ColorTheme
{
    public abstract Color PrimaryText { get; }
    public abstract Color ItemSlotTextColor { get; }

    public abstract Color PanelBackgroundColor { get; }
    public abstract Color PanelForegroundColor { get; }

    public abstract Color PanelOutline { get; }

    public abstract Color OccupiedInventorySlot { get; }
    public abstract Color SelectedInventorySlot { get; }
    public abstract Color GridDivider { get; }
    public abstract Color PanelGradientStart { get; }
    public abstract Color PanelGradientEnd { get; }
}