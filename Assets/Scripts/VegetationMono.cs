using Core;
using HighlightPlus;
using UnityEngine;
using UnityEngine.Rendering;

public class VegetationMono : MonoBehaviour, Interactable
{
    public Point2Int Pos;
    public TerrainObjectType Type;
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
    public GameObject GameObject => this.gameObject;

    public void Init(Point2Int pos, TerrainObjectType type)
    {
        this.Type = type;
        this.Pos = pos;
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
        if (this.Type == TerrainObjectType.Bush)
        {
            PlayerMono.Instance.PluckBush(this.Pos);
        }
        else
        {
            Debug.Log("Interaction click on " + Type);
        }
    }
}