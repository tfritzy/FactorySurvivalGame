using System.Collections.Generic;
using Core;
using UnityEngine;

public static class Icons
{
    private static Dictionary<ItemType, Sprite> _icons = new Dictionary<ItemType, Sprite>();
    public static Sprite GetIcon(ItemType type)
    {
        if (_icons == null)
            _icons = new Dictionary<ItemType, Sprite>();

        if (!_icons.ContainsKey(type))
        {
            _icons[type] = Resources.Load<Sprite>("Icons/" + type.ToString());
        }

        return _icons[type];
    }
}