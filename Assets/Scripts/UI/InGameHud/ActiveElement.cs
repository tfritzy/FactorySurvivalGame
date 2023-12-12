using UnityEngine.UIElements;

public abstract class ActiveElement : VisualElement
{
    public bool Shown { get; protected set; } = true;
    public virtual void Update() { }
    public VisualElement Content { get; protected set; }

    public ActiveElement()
    {
        this.Content = this;
    }

    public void ToggleShown()
    {
        Shown = !Shown;

        if (Shown)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    public virtual void Show()
    {
        this.style.display = DisplayStyle.Flex;
        Shown = true;
    }

    public virtual void Hide()
    {
        this.style.display = DisplayStyle.None;
        Shown = false;
    }
}