using Core;
using UnityEngine;

public class CharacterMono : EntityMono
{
    private bool previewMode;
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
        };
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
        foreach (Collider c in GetComponentsInChildren<Collider>())
        {
            c.enabled = false;
        }
    }

    private void SetNonPreview()
    {
        previewMode = false;
        foreach (Collider c in GetComponentsInChildren<Collider>())
        {
            c.enabled = true;
        }
    }
}