using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform player;
    Vector3 offset;
    public float speed = 10f;

    void Start()
    {
        offset = transform.position - player.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position + offset, Time.deltaTime * speed);
    }
}
