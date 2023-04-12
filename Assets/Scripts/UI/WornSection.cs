using UnityEngine;
using UnityEngine.UIElements;

public class WornItemsSection : VisualElement
{
    public WornItemsSection()
    {
        this.style.width = Length.Percent(100);
        this.style.backgroundColor = Color.green;
    }
}