using System.Collections.Generic;
using Core;
using UnityEngine;

public static class Models
{
    private static Dictionary<TriangleSubType, GameObject> _subTypeMeshes = new Dictionary<TriangleSubType, GameObject>();
    public static GameObject GetTriangleMesh(TriangleSubType type)
    {
        if (_subTypeMeshes == null)
            _subTypeMeshes = new Dictionary<TriangleSubType, GameObject>();

        if (!_subTypeMeshes.ContainsKey(type))
        {
            _subTypeMeshes[type] = Resources.Load<GameObject>("Prefabs/Triangles/" + type.ToString());
        }

        return _subTypeMeshes[type];
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

    private static Dictionary<ItemType, GameObject> _itemModels = new Dictionary<ItemType, GameObject>();
    public static GameObject GetItemModel(ItemType type)
    {
        if (_itemModels == null)
            _itemModels = new Dictionary<ItemType, GameObject>();

        if (!_itemModels.ContainsKey(type))
        {
            _itemModels[type] = Resources.Load<GameObject>("Prefabs/Items/" + type.ToString());
        }

        return _itemModels[type];
    }

    private static Dictionary<VegetationType, GameObject[]> _vegetationPrefabs = new();
    public static GameObject GetVegetationPrefab(VegetationType type)
    {
        if (_vegetationPrefabs == null)
            _vegetationPrefabs = new Dictionary<VegetationType, GameObject[]>();

        if (!_vegetationPrefabs.ContainsKey(type))
        {
            _vegetationPrefabs[type] = Resources.LoadAll<GameObject>("Prefabs/Vegetation/" + type.ToString());
        }

        if (_vegetationPrefabs[type].Length == 0)
        {
            throw new System.Exception("Could not find a folder of vegetation for " + type);
        }

        return _vegetationPrefabs[type][Random.Range(0, _vegetationPrefabs[type].Length)];
    }
}