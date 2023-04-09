using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

public static class Helpers
{
    public static GameObject GetFromPoolOrCreate(GameObject prefab, List<GameObject> pool, Transform parent)
    {
        if (pool?.Count > 0)
        {
            GameObject poolObj = pool.Last();
            poolObj.SetActive(true);
            pool.RemoveAt(pool.Count - 1);
            poolObj.transform.position = parent.transform.position;
            return poolObj;
        }
        else
        {
            return GameObject.Instantiate(
                prefab,
                parent.transform.position,
                Quaternion.identity,
                parent
            );
        }
    }
}