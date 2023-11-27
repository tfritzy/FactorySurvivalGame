using Core;
using UnityEngine;

public class CharacterMono : EntityMono
{
    public override void Setup(Entity entity)
    {
        base.Setup(entity);

        foreach (Collider c in GetComponentsInChildren<Collider>(includeInactive: true))
        {
            c.gameObject.layer = Layers.Character;
        }
    }
}