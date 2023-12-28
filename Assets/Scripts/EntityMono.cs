using Core;
using HighlightPlus;
using UnityEngine;

public abstract class EntityMono : MonoBehaviour
{
    public virtual void Tick(float deltaTime) { }
    public Entity Actual { get; protected set; }
    public HighlightEffect HighlightEffect { get; private set; }

    public virtual void Setup(Entity entity)
    {
        this.Actual = entity;
        HighlightEffect = gameObject.AddComponent<HighlightEffect>();
        HighlightEffect.ProfileLoad(HighlightProfiles.GetHighlightProfile(HighlightProfiles.Profile.Highlighted));
    }

    void Update()
    {
        Tick(Time.deltaTime);
    }
}