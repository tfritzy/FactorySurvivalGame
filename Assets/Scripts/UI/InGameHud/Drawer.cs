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
        this.style.backgroundColor = Color.red;
        this.SetAllPadding(10);
    }
}