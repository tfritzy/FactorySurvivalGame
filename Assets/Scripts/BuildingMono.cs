using System.Collections.Generic;
using System.Linq;
using Core;
using DG.Tweening;
using UnityEngine;

public class BuildingMono : CharacterMono
{
    private Building ActualBuilding => (Building)this.Actual;
    private HexSide? rotation;

    public override void Setup(Entity entity)
    {
        base.Setup(entity);
        transform.rotation = Quaternion.Euler(0, (int)ActualBuilding.Rotation * 60, 0);
        rotation = ActualBuilding.Rotation;
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);

        if (ActualBuilding.Rotation != rotation)
        {
            rotation = ActualBuilding.Rotation;
            transform.DORotate(new Vector3(0, (int)rotation * 60, 0), 0.2f, RotateMode.Fast);
        }
    }
}