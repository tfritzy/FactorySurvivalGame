using System.Collections.Generic;
using UnityEngine;

public static class Models
{
    private static Dictionary<PrefabType, GameObject> _hexModels = new Dictionary<PrefabType, GameObject>();
    public static GameObject GetHexModel(PrefabType type)
    {
        if (_hexModels == null)
            _hexModels = new Dictionary<PrefabType, GameObject>();

        if (!_hexModels.ContainsKey(type))
        {
            _hexModels[type] = Resources.Load<GameObject>("Prefabs/" + type.ToString());
        }

        return _hexModels[type];
    }
}