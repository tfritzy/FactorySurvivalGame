using HighlightPlus;
using UnityEngine;

public interface Interactable
{
    public void OnInteract();
    public void OnInspect();
    public HighlightEffect GetHighlightEffect();
    public GameObject GameObject { get; }
}