using UnityEngine;

public class DarkTheme : ColorTheme
{
    private static Color primaryText = new Color(0.9f, 0.9f, 0.9f);
    public override Color PrimaryText => primaryText;
    private static Color itemSlotTextColor = new Color(0.9f, 0.9f, 0.9f);
    public override Color ItemSlotTextColor => itemSlotTextColor;

    private static Color panelBackgroundColor = new Color(0.1f, 0.1f, 0.1f);
    public override Color PanelBackgroundColor => panelBackgroundColor;
    private static Color panelForegroundColor = new Color(0.15f, 0.15f, 0.15f);
    public override Color PanelForegroundColor => panelForegroundColor;

    private static Color panelOutlineColorMid = new Color(0.3f, 0.3f, 0.3f);
    private static Color panelOutlineColorBright = new Color(0.4f, 0.4f, 0.4f);
    private static Color panelOutlineColorDark = new Color(0.2f, 0.2f, 0.2f);
    public override Color PanelOutlineColorMid => panelOutlineColorMid;
    public override Color PanelOutlineColorBright => panelOutlineColorBright;
    public override Color PanelOutlineColorDark => panelOutlineColorDark;

    private static Color occupiedInventorySlot = new Color(.6f, .6f, .6f, .06f);
    public override Color OccupiedInventorySlot => occupiedInventorySlot;
    private static Color selectedInventorySlot = ColorExtensions.FromHex("#662a2a");
    public override Color SelectedInventorySlot => selectedInventorySlot;

    private static Color panelGradientStart = ColorExtensions.FromHex("#02120a");
    public override Color PanelGradientStart => panelGradientStart;
    private static Color panelGradientEnd = ColorExtensions.FromHex("#001529");
    public override Color PanelGradientEnd => panelGradientEnd;
}