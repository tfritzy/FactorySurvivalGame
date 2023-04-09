using System.Collections.Generic;
using Core;
using UnityEngine;

public class ItemMono : EntityMono
{
    public Item Actual;

    private static Dictionary<ItemType, List<GameObject>> ItemPool = new Dictionary<ItemType, List<GameObject>>();

    public GameObject body;

    public override void Tick(float deltaTime) { }

    public override void Spawn()
    {
        ItemPool.TryGetValue(this.Actual.Type, out List<GameObject> pool);
        this.body = Helpers.GetFromPoolOrCreate(Models.GetItemModel(this.Actual.Type), pool, this.transform);
    }

    public override void Despawn()
    {
        this.body.SetActive(false);
        this.body.transform.SetParent(null);
        if (!ItemPool.ContainsKey(this.Actual.Type))
            ItemPool[this.Actual.Type] = new List<GameObject>();
        ItemPool[this.Actual.Type].Add(this.body);
        Destroy(this.gameObject);
    }
}