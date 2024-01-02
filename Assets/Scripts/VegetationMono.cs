using Core;
using HighlightPlus;
using UnityEngine;
using UnityEngine.Rendering;

public class VegetationMono : MonoBehaviour, Interactable
{
    private HighlightEffect? _highlightEffect;
    private HighlightEffect HighlightEffect
    {
        get
        {
            if (_highlightEffect == null)
            {
                _highlightEffect = GetComponent<HighlightEffect>();
            }

            return _highlightEffect;
        }
    }

    public HighlightEffect GetHighlightEffect()
    {
        return HighlightEffect;
    }

    public void OnInspect()
    {
        Debug.Log("Inspection click on " + name);
    }

    public void OnInteract()
    {
        PlayerMono.Instance.MoveCommand(transform.position);
    }
}