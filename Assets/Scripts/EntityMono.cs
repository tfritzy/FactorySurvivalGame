using Core;
using UnityEngine;

public abstract class EntityMono : MonoBehaviour
{
    public virtual void Tick(float deltaTime) { }
    public virtual void Setup(Entity entity)
    {
        this.Actual = entity;
    }
    protected Entity Actual;


    void Update()
    {
        Tick(Time.deltaTime);
    }
}