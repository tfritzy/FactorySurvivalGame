using UnityEngine;
using UnityEngine.UIElements;

public abstract class Drawer : ActiveElement
{
    public Drawer()
    {
        this.Content.style.position = Position.Absolute;
        this.Content.style.right = 0;
        this.Content.style.top = 0;
        this.Content.style.height = Length.Percent(100);
        this.Content.style.backgroundColor = UIManager.ColorTheme.PanelBackgroundColor;
        this.Content.SetAllPadding(10);
        this.Content.style.borderLeftWidth = 3;
        this.Content.style.borderLeftColor = UIManager.ColorTheme.PanelOutlineColorBright;
    }
}