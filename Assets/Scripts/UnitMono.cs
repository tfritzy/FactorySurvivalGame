using Core;
using UnityEngine;

public class UnitMono : CharacterMono
{
    private new Unit Actual => (Unit)base.Actual;

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);
        this.transform.position = Actual.Location.ToVector3();
    }
}