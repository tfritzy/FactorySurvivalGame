using UnityEngine.UIElements;

public abstract class ActiveElement : VisualElement
{
    public abstract void Update();

    public void ToggleShown()
    {
        this.style.display =
            this.style.display == DisplayStyle.Flex ?
                DisplayStyle.None :
                DisplayStyle.Flex;
    }
}