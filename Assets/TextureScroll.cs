using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScroll : MonoBehaviour
{
    public bool Reversed = false;
    private Material material;

    void Update()
    {
        if (material == null)
        {
            material = GetComponent<Renderer>().material;
        }

        Vector2 offset = material.mainTextureOffset;
        offset.x += (Reversed ? -1 : 1) * Time.deltaTime / 10;
        material.mainTextureOffset = offset;
    }
}
