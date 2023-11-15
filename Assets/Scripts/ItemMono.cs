using System.Collections.Generic;
using Core;
using UnityEngine;

public class ItemMono : EntityMono
{
    public Item Actual;

    private static Dictionary<ItemType, List<GameObject>> ItemPool = new Dictionary<ItemType, List<GameObject>>();

    public GameObject body;

}