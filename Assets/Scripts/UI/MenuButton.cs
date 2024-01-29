using UnityEngine.UIElements;

public class MenuButton : Button
{
    public MenuButton(string text, System.Action onClick)
    {
        this.text = text;
        style.marginTop = 10;
        style.width = 200;
        style.height = 30;
        style.alignSelf = Align.Center;
        style.backgroundColor = ColorTheme.Current.PanelForegroundColor;
        style.color = ColorTheme.Current.PrimaryText;
        this.SetAllBorderRadius(5);
        clicked += onClick;
    }
}