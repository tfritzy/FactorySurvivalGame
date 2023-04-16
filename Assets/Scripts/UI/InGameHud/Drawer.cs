using UnityEngine;
using UnityEngine.UIElements;

public abstract class Drawer : ActiveElement
{
    public Drawer()
    {
        this.style.position = Position.Absolute;
        this.style.right = 0;
        this.style.top = 0;
        this.style.height = Length.Percent(100);
        this.style.backgroundColor = UI.ColorTheme.PanelBackgroundColor;
        UI.ColorTheme.ApplyPanelBorderColor(this);
        this.style.borderTopLeftRadius = 10;
        this.style.borderBottomLeftRadius = 10;
        this.SetAllBorderWidth(1);
        this.SetAllPadding(10);
    }
}