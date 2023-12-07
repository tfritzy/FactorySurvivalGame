using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float roatateDegreesPerSecond = 180f;
    public Vector3 rotationAxis = Vector3.up;
    private Vector3 startRotation;

    void Start()
    {
        startRotation = transform.rotation.eulerAngles;
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(startRotation + rotationAxis * roatateDegreesPerSecond * Time.time);
    }
}
