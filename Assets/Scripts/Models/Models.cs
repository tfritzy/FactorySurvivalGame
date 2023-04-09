using System.Collections.Generic;
using Core;
using UnityEngine;

public static class Models
{
    private static Dictionary<HexType, GameObject> _hexModels = new Dictionary<HexType, GameObject>();
    public static GameObject GetHexModel(HexType type)
    {
        if (_hexModels == null)
            _hexModels = new Dictionary<HexType, GameObject>();

        if (!_hexModels.ContainsKey(type))
        {
            _hexModels[type] = Resources.Load<GameObject>("Prefabs/Hexes/" + type.ToString());
        }

        Debug.Log("Returning hex model " + _hexModels[type]?.name);

        return _hexModels[type];
    }

    private static Dictionary<CharacterType, GameObject> _characterModels = new Dictionary<CharacterType, GameObject>();
    public static GameObject GetCharacterModel(CharacterType type)
    {
        if (_characterModels == null)
            _characterModels = new Dictionary<CharacterType, GameObject>();

        if (!_characterModels.ContainsKey(type))
        {
            _characterModels[type] = Resources.Load<GameObject>("Prefabs/Characters/" + type.ToString());
        }

        return _characterModels[type];
    }
}