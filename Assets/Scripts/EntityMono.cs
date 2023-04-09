using Core;
using UnityEngine;

public abstract class EntityMono : MonoBehaviour
{
    public abstract void Spawn();
    public abstract void Despawn();
    public abstract void Tick(float deltaTime);
}