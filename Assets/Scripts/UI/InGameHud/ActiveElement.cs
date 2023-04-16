using UnityEngine.UIElements;

public abstract class ActiveElement : VisualElement
{
    public bool Shown { get; private set; } = true;
    public abstract void Update();

    public void ToggleShown()
    {
        Shown = !Shown;

        if (Shown)
        {
            this.style.display = DisplayStyle.Flex;
        }
        else
        {
            this.style.display = DisplayStyle.None;
        }
    }
}