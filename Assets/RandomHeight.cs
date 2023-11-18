using UnityEngine;

public class RandomHeight : MonoBehaviour
{
    void Start()
    {
        var currentScale = transform.localScale;
        float newY = Random.Range(0.8f, 1.2f);
        transform.localScale = new Vector3(currentScale.x, newY, currentScale.z);
        Destroy(this);
    }
}