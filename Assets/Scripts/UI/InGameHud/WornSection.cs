using UnityEngine;
using UnityEngine.UIElements;

public class WornItemsSection : ActiveElement
{
    public WornItemsSection()
    {
        this.style.width = Length.Percent(100);
        this.style.backgroundColor = Color.green;
    }

    public override void Update()
    {
    }
}