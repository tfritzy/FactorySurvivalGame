using UnityEngine;
using UnityEngine.UIElements;

public class Drawer : VisualElement
{
    public Drawer()
    {
        this.style.position = Position.Absolute;
        this.style.right = 0;
        this.style.top = 0;

        this.style.width = 600;
        this.style.height = Length.Percent(100);

        this.style.paddingBottom = 10;
        this.style.paddingTop = 10;
        this.style.paddingLeft = 10;
        this.style.paddingRight = 10;

        this.style.backgroundColor = Color.red;
    }
}