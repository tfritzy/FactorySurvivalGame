using UnityEngine;

public class DarkTheme : ColorTheme
{
    private static Color primaryText = Color.black;
    public override Color PrimaryText => primaryText;
    private static Color itemSlotTextColor = Color.black;
    public override Color ItemSlotTextColor => itemSlotTextColor;

    private static Color panelBackgroundColor = Color.white;
    public override Color PanelBackgroundColor => panelBackgroundColor;
    private static Color panelForegroundColor = new Color(0.95f, 0.95f, 0.95f);
    public override Color PanelForegroundColor => panelForegroundColor;

    private static Color panelOutline = Color.black;
    public override Color PanelOutline => panelOutline;
    private static Color gridDivider = new Color(.90f, .90f, .90f);
    public override Color GridDivider => gridDivider;

    private static Color occupiedInventorySlot = new Color(.95f, .95f, .95f);
    public override Color OccupiedInventorySlot => occupiedInventorySlot;
    private static Color selectedInventorySlot = ColorExtensions.FromHex("#b3b3ff");
    public override Color SelectedInventorySlot => selectedInventorySlot;

    private static Color panelGradientStart = Color.white;
    public override Color PanelGradientStart => panelGradientStart;
    private static Color panelGradientEnd = ColorExtensions.FromHex("#e6e6e6");
    public override Color PanelGradientEnd => panelGradientEnd;
}