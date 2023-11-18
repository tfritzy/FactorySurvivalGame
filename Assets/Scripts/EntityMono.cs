using Core;
using UnityEngine;

public abstract class EntityMono : MonoBehaviour
{
    public virtual void Tick(float deltaTime) { }
    public virtual void Setup(Entity entity)
    {
        this.Actual = entity;
    }
    public Entity Actual { get; protected set; }


    void Update()
    {
        Tick(Time.deltaTime);
    }
}