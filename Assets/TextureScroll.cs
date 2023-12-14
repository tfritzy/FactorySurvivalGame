using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScroll : MonoBehaviour
{
    public bool Reversed = false;
    public float Speed = 4;
    private new Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        Vector2 offset = renderer.material.mainTextureOffset;
        offset.x += (Reversed ? -1 : 1) * Time.deltaTime / Speed;
        renderer.material.mainTextureOffset = offset;
    }
}
