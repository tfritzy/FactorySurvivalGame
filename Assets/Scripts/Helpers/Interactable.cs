using HighlightPlus;

public interface Interactable
{
    public void OnInteract();
    public void OnInspect();
    public HighlightEffect GetHighlightEffect();
}