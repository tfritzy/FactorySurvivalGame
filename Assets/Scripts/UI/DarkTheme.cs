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

    private static Color panelOutlineColorMid = new Color(0.8f, 0.8f, 0.8f);
    private static Color panelOutlineColorBright = new Color(0.7f, 0.7f, 0.7f);
    private static Color panelOutlineColorDark = new Color(0.9f, 0.9f, 0.9f);
    public override Color PanelOutlineColorMid => panelOutlineColorMid;
    public override Color PanelOutlineColorBright => panelOutlineColorBright;
    public override Color PanelOutlineColorDark => panelOutlineColorDark;

    private static Color occupiedInventorySlot = new Color(.75f, .75f, .75f, .075f);
    public override Color OccupiedInventorySlot => occupiedInventorySlot;
    private static Color selectedInventorySlot = ColorExtensions.FromHex("#b3b3ff");
    public override Color SelectedInventorySlot => selectedInventorySlot;

    private static Color panelGradientStart = Color.white;
    public override Color PanelGradientStart => panelGradientStart;
    private static Color panelGradientEnd = ColorExtensions.FromHex("#e6e6e6");
    public override Color PanelGradientEnd => panelGradientEnd;
}