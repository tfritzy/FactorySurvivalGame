using System.Collections.Generic;
using UnityEngine;

public static class Models
{
    private static Dictionary<HexModelType, GameObject> _hexModels = new Dictionary<HexModelType, GameObject>();
    public static GameObject GetHexModel(HexModelType type)
    {
        if (_hexModels == null)
            _hexModels = new Dictionary<HexModelType, GameObject>();

        if (!_hexModels.ContainsKey(type))
        {
            _hexModels[type] = Resources.Load<GameObject>("Models/Hexes/" + type.ToString());
        }

        return _hexModels[type];
    }
}