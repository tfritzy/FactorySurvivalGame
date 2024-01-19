using System.Collections.Generic;
using Core;
using UnityEngine;

public static class HexPool
{
    private static Dictionary<TriangleSubType, Queue<GameObject>> Hexes = new();
    public static GameObject GetTri(TriangleSubType tri, Transform? parent)
    {
        if (!Hexes.ContainsKey(tri))
        {
            Hexes[tri] = new Queue<GameObject>();
        }
        if (Hexes[tri].Count == 0)
        {
            var triObj = GameObject.Instantiate(Models.GetTriangleMesh(tri), parent);
            triObj.name = tri.ToString();
            Hexes[tri].Enqueue(triObj);
        }
        var hex = Hexes[tri].Dequeue();
        hex.SetActive(true);
        return hex;
    }

    public static void ReturnTri(TriangleSubType tri, GameObject hex)
    {
        if (!Hexes.ContainsKey(tri))
        {
            Hexes[tri] = new Queue<GameObject>();
        }

        hex.SetActive(false);
        Hexes[tri].Enqueue(hex);
    }
}