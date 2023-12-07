using System.Collections.Generic;
using UnityEngine;

public static class DecalLoader
{
    public enum Decal
    {
        BuildGridHex,
        BuildGridHexFilled,
        Arrow
    };

    private static Dictionary<Decal, GameObject> Decals = new();
    public static GameObject GetDecalPrefab(Decal type)
    {
        if (!Decals.ContainsKey(type))
        {
            Decals[type] = Resources.Load<GameObject>($"Prefabs/Decals/{type}");
        }
        return Decals[type];
    }
}