using Core;
using UnityEngine;

public class ItemMono : MonoBehaviour
{
    public Item Item { get; private set; }

    public void SetItem(Item item)
    {
        Item = item;
    }
}