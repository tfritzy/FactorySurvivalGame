using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

public class ConveyorMono : BuildingMono
{
    public Texture2D ConveyorForwardTexture;
    public Texture2D ConveyorBackwardsTexture;

    private Building owner => (Building)this.Actual;
    private ConveyorComponent conveyor => owner.GetComponent<ConveyorComponent>();
    private GameObject _straightBody;
    public GameObject StraightBody
    {
        get
        {
            if (_straightBody == null)
            {
                _straightBody = this.transform.Find("Straight").gameObject;
            }
            return _straightBody;

        }
    }
    private GameObject _curvedBody;
    public GameObject CurvedBody
    {
        get
        {
            if (_curvedBody == null)
            {
                _curvedBody = this.transform.Find("Curved").gameObject;
            }
            return _curvedBody;

        }
    }

    public override void Setup(Entity entity)
    {
        base.Setup(entity);
        UpdateOwnBody();
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);
        UpdateOwnBody();
    }

    private bool? cachedCurved;
    private HexSide? cachedPrev;
    private HexSide? cachedRotation;
    public void UpdateOwnBody()
    {
        if (Actual.Conveyor.IsCurved() != cachedCurved ||
            Actual.Conveyor.PrevSide != cachedPrev
            || owner.Rotation != cachedRotation)
        {
            cachedPrev = Actual.Conveyor.PrevSide;
            cachedRotation = owner.Rotation;
            cachedCurved = Actual.Conveyor.IsCurved();
            if (cachedCurved.Value)
            {
                CurvedBody.SetActive(true);
                StraightBody.gameObject.SetActive(false);

                int inSide = (int)Actual.Conveyor.PrevSide;
                int outSide = (int)owner.Rotation;
                if (outSide < 2 && inSide > 3)
                    outSide += 6;
                if (inSide > 3 && outSide < 2)
                    inSide -= 6;
                int delta = outSide - inSide;
                bool flipped = delta != 2;

                // var rotation = ((int)Actual.Conveyor.PrevSide + (flipped ? 4 : 0)) * 60;
                var rotation = (flipped ? 0 : 4) * 60;
                CurvedBody.transform.localRotation = Quaternion.Euler(0, rotation, 0);

                GetComponentInChildren<TextureScroll>().Reversed = flipped;
                CurvedBody.transform.Find("Belt").GetComponent<MeshRenderer>().material.mainTexture
                    = flipped ? ConveyorBackwardsTexture : ConveyorForwardTexture;
            }
            else
            {
                StraightBody.gameObject.SetActive(true);
                CurvedBody.SetActive(false);
            }
        }
    }
}