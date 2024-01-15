using Core;
using HighlightPlus;
using UnityEngine;

public class ItemMono : MonoBehaviour, Interactable
{
    public Item Item { get; private set; }
    public GameObject GameObject => this.gameObject;
    private HighlightEffect? highlightEffect;

    public HighlightEffect GetHighlightEffect()
    {
        return highlightEffect!;
    }

    void Awake()
    {
        highlightEffect = this.gameObject.AddComponent<HighlightEffect>();
        highlightEffect.ProfileLoad(HighlightProfiles.GetHighlightProfile(HighlightProfiles.Profile.Highlighted));
        gameObject.layer = Layers.Item;
        var child = transform.GetChild(0).gameObject;
        child.layer = Layers.Item;
        var collider = child.AddComponent<SphereCollider>();
        collider.radius = 0.5f;
        collider.isTrigger = true;
        this.transform.localScale *= 1.2f;
        this.transform.position += Vector3.up * 0.2f;
    }

    public void OnInspect()
    {
        Debug.Log("Inspecting " + Item.Name);
    }

    public void OnInteract()
    {
        PlayerMono.Instance.PickupItem(this.Item.Id);
    }

    public void SetItem(Item item)
    {
        Item = item;
    }
}