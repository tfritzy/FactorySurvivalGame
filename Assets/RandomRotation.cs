using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    void Start()
    {
        var currentRotation = transform.rotation;
        float newY = Random.Range(0, 360);
        transform.rotation = Quaternion.Euler(currentRotation.x, newY, currentRotation.z);
        transform.position += new Vector3(Random.Range(-0.1f, 0.1f), 0, Random.Range(-0.1f, 0.1f));
        Destroy(this);
    }
}
