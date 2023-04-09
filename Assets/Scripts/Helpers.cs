using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

public static class Helpers
{
    public static GameObject GetFromPoolOrCreate(GameObject prefab, List<GameObject> pool, Point3Int point, Transform parent)
    {
        Debug.Log("GetFromPoolOrCreate");
        if (pool?.Count > 0)
        {
            GameObject poolObj = pool.Last();
            poolObj.SetActive(true);
            pool.RemoveAt(pool.Count - 1);
            poolObj.transform.position = WorldConversions.HexToUnityPosition(point);
            return poolObj;
        }
        else
        {
            return GameObject.Instantiate(
                prefab,
                WorldConversions.HexToUnityPosition(point),
                Quaternion.identity,
                parent
            );
        }
    }
}