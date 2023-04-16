using UnityEngine;
using UnityEngine.UIElements;

public abstract class Modal : ActiveElement
{
    protected VisualElement modal;

    public Modal()
    {
        this.style.position = Position.Absolute;
        this.style.left = 0;
        this.style.top = 0;
        this.style.width = Length.Percent(100);
        this.style.height = Length.Percent(100);
        this.style.alignItems = Align.Center;
        this.style.justifyContent = Justify.Center;
        this.pickingMode = PickingMode.Ignore;

        this.modal = new VisualElement();
        this.modal.style.backgroundColor = Color.magenta;
        this.Add(this.modal);
    }
}