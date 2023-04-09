using Core;
using UnityEngine;

public abstract class EntityMono : MonoBehaviour
{
    public Entity Entity;

    public abstract void Spawn();
    public abstract void Despawn();
}