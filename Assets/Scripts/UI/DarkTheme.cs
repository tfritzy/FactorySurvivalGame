using UnityEngine;

public class DarkTheme : ColorTheme
{
    private static Color primaryText = ColorExtensions.FromHex("#e6eeed");
    public override Color PrimaryText => primaryText;
    private static Color itemSlotTextColor = ColorExtensions.FromHex("#e6eeed");
    public override Color ItemSlotTextColor => itemSlotTextColor;

    private static Color panelBackgroundColor = ColorExtensions.FromHex("#2e2e43");
    public override Color PanelBackgroundColor => panelBackgroundColor;
    private static Color panelForegroundColor = ColorExtensions.FromHex("#4a4b5b");
    public override Color PanelForegroundColor => panelForegroundColor;

    private static Color panelOutline = ColorExtensions.FromHex("#707b89");
    public override Color PanelOutline => panelOutline;
    private static Color gridDivider = ColorExtensions.FromHex("#707b89");
    public override Color GridDivider => gridDivider;

    private static Color occupiedInventorySlot = ColorExtensions.FromHex("#2e2e43");
    public override Color OccupiedInventorySlot => occupiedInventorySlot;
    private static Color selectedInventorySlot = ColorExtensions.FromHex("#9d4343");
    public override Color SelectedInventorySlot => selectedInventorySlot;

    private static Color mainAccent = ColorExtensions.FromHex("#c06852");
    public override Color MainAccent => mainAccent;
}