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
            var itemObj = GameObject.Instantiate(Models.GetItemModel(type), parent);
            itemObj.AddComponent<ItemMono>();
            itemObj.name = type.ToString();
            Items[type].Enqueue(itemObj);
        }
        var item = Items[type].Dequeue();
        item.transform.SetParent(parent);
        item.SetActive(true);
        return item;
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