using UnityEngine;

public class LightTheme : ColorTheme
{
    private static Color primaryText = new Color(0.1f, 0.1f, 0.1f);
    public override Color PrimaryText => primaryText;
    private static Color itemSlotTextColor = new Color(0.1f, 0.1f, 0.1f);
    public override Color ItemSlotTextColor => itemSlotTextColor;

    private static Color panelBackgroundColor = new Color(1f, 1f, 1f);
    public override Color PanelBackgroundColor => panelBackgroundColor;
    private static Color panelForegroundColor = new Color(0.8f, 0.8f, 0.8f);
    public override Color PanelForegroundColor => panelForegroundColor;

    private static Color panelOutlineColorMid = new Color(0.7f, 0.7f, 0.7f);
    private static Color panelOutlineColorBright = new Color(0.75f, 0.75f, 0.75f);
    private static Color panelOutlineColorDark = new Color(0.65f, 0.65f, 0.65f);
    public override Color PanelOutlineColorMid => panelOutlineColorMid;
    public override Color PanelOutlineColorBright => panelOutlineColorBright;
    public override Color PanelOutlineColorDark => panelOutlineColorDark;

    private static Color occupiedInventorySlot = new Color(0.6f, 0.6f, 0.6f);
    public override Color OccupiedInventorySlot => occupiedInventorySlot;
    private static Color selectedInventorySlot = ColorExtensions.FromHex("#662a2a");
    public override Color SelectedInventorySlot => selectedInventorySlot;
}