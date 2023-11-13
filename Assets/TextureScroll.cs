using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScroll : MonoBehaviour
{
    private Material material;

    void Update()
    {
        if (material == null)
        {
            material = GetComponent<Renderer>().material;
        }

        Vector2 offset = material.mainTextureOffset;
        offset.x -= Time.deltaTime / 10;
        material.mainTextureOffset = offset;
    }
}
