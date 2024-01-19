using System.Collections.Generic;
using Core;
using UnityEngine;

public static class VegetationPool
{
    private static Dictionary<TerrainObjectType, Queue<GameObject>> Vegetation = new();
    public static GameObject GetVegetation(TerrainObjectType type, Transform? parent)
    {
        if (!Vegetation.ContainsKey(type))
        {
            Vegetation[type] = new Queue<GameObject>();
        }
        if (Vegetation[type].Count == 0)
        {
            var triObj = GameObject.Instantiate(Models.GetVegetationPrefab(type), parent);
            triObj.name = type.ToString();
            Vegetation[type].Enqueue(triObj);
        }
        var vege = Vegetation[type].Dequeue();
        vege.SetActive(true);
        return vege;
    }

    public static void ReturnVegetation(TerrainObjectType type, GameObject vegetation)
    {
        if (!Vegetation.ContainsKey(type))
        {
            Vegetation[type] = new Queue<GameObject>();
        }

        vegetation.SetActive(false);
        Vegetation[type].Enqueue(vegetation);
    }
}