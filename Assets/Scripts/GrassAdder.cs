using UnityEngine;

public class GrassAdder : MonoBehaviour
{
    public Sprite Grass;

    void Start()
    {
        var grass = new GameObject("Grass");
        grass.transform.SetParent(this.transform);
        var sr = grass.AddComponent<SpriteRenderer>();
        sr.sprite = Grass;
        Vector3 pos = Random.insideUnitSphere;
        pos.z = sr.localBounds.max.y;
        grass.transform.localPosition = pos;
    }
}