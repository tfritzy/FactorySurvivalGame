using System.Collections.Generic;
using Core;
using UnityEngine;

public class ItemPool
{
    private static Dictionary<ItemType, Queue<GameObject>> Items = new();
    public static GameObject GetItem(ItemType type, Transform parent)
    {
        if (!Items.ContainsKey(type))
        {
            Items[type] = new Queue<GameObject>();
        }
        if (Items[type].Count == 0)
        {
            var model = Models.GetItemModel(type);
            if (model == null)
            {
                throw new System.Exception("Missing prefab for item: " + type);
            }

            var newObj = GameObject.Instantiate(model, parent);
            newObj.AddComponent<ItemMono>();
            newObj.name = type.ToString();
            Items[type].Enqueue(newObj);
        }
        var itemObj = Items[type].Dequeue();
        itemObj.transform.SetParent(parent);
        itemObj.SetActive(true);
        return itemObj;
    }

    public static void ReturnItem(ItemType type, GameObject item)
    {
        if (!Items.ContainsKey(type))
        {
            Items[type] = new Queue<GameObject>();
        }

        item.SetActive(false);
        Items[type].Enqueue(item);
    }
}