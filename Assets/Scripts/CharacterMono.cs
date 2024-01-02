using System.Collections.Generic;
using System.Linq;
using Core;
using HighlightPlus;
using UnityEngine;

public class CharacterMono : EntityMono, Interactable
{
    private bool previewMode;
    private List<Renderer> renderers = new List<Renderer>();
    private float birthTime;

    public override void Setup(Entity entity)
    {
        base.Setup(entity);

        foreach (Collider c in GetComponentsInChildren<Collider>(includeInactive: true))
        {
            c.gameObject.layer = Layers.Character;
        }

        renderers = GetComponentsInChildren<Renderer>(includeInactive: true).ToList();

        if (((Character)Actual).IsPreview)
        {
            SetPreview();
        };

        birthTime = Time.time;
        name = ((Character)Actual).Type + "_" + Actual.Id;
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);

        if (previewMode && !((Character)Actual).IsPreview)
        {
            SetNonPreview();
        }
    }

    private void SetPreview()
    {
        previewMode = true;
        foreach (Collider c in GetComponentsInChildren<Collider>(includeInactive: true))
        {
            c.enabled = false;
        }
    }


    private void SetNonPreview()
    {
        birthTime = Time.time;
        previewMode = false;
        foreach (Collider c in GetComponentsInChildren<Collider>(includeInactive: true))
        {
            c.enabled = true;
        }
    }

    public void OnInteract()
    {
        Debug.Log("Interraction click on " + name);
    }

    public HighlightEffect GetHighlightEffect()
    {
        return HighlightEffect;
    }

    public void OnInspect()
    {
        UIManager.Instance.OpenCharacterInspector((Character)Actual);
    }
}