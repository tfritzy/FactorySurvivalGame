using UnityEngine.UIElements;

public abstract class ActiveElement : VisualElement
{
    public bool Shown { get; private set; } = true;
    public abstract void Update();
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
    }

    public virtual void Hide()
    {
        this.style.display = DisplayStyle.None;
    }
}