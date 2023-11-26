using System.Collections.Generic;
using Core;
using UnityEngine;

public class CharacterMono : EntityMono
{
    private bool isRenderedAsPreview = false;

    public override void Setup(Entity entity)
    {
        base.Setup(entity);

        foreach (Collider c in GetComponentsInChildren<Collider>(includeInactive: true))
        {
            c.gameObject.layer = Layers.Character;
        }

        if (((Character)Actual).IsPreview)
        {
            SetPreview();
            isRenderedAsPreview = true;
        }
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);

        if (!((Character)Actual).IsPreview && isRenderedAsPreview)
        {
            RestoreFromPreview();
            isRenderedAsPreview = false;
        }
    }

    private Dictionary<GameObject, Material> originalMaterials = new Dictionary<GameObject, Material>();
    private void SetPreview()
    {
        foreach (Renderer r in gameObject.GetComponentsInChildren<Renderer>(true))
        {
            originalMaterials.Add(r.gameObject, r.material);
            r.material = MaterialDepot.GetMaterial(MaterialDepot.Material.Preview);
        }

        foreach (Collider c in GetComponentsInChildren<Collider>(includeInactive: true))
        {
            c.enabled = false;
        }
    }

    private void RestoreFromPreview()
    {
        foreach (Renderer r in gameObject.GetComponentsInChildren<Renderer>(true))
        {
            if (originalMaterials.ContainsKey(r.gameObject))
                r.material = originalMaterials[r.gameObject];
        }

        foreach (Collider c in GetComponentsInChildren<Collider>(includeInactive: true))
        {
            c.enabled = true;
        }
    }
}