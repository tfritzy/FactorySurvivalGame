using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowGameobject : MonoBehaviour
{
    public GameObject Target;

    void LateUpdate()
    {
        if (Target == null)
        {
            return;
        }

        float distanceFromCamera = Vector3.Distance(Managers.MainCamera.transform.position, Target.transform.position);
        float scale = Mathf.Clamp(1 - (distanceFromCamera - 5) / 10, 0.5f, 1f);
        transform.localScale = new Vector3(scale, scale, scale);
        var screenPosition = Managers.MainCamera.WorldToScreenPoint(Target.transform.position);
        transform.position = screenPosition;
    }
}
