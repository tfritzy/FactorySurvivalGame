using System.Collections.Generic;
using Core;
using UnityEngine;

public abstract class CharacterMono : EntityMono
{
    public Character Actual;

    private static Dictionary<CharacterType, List<GameObject>> CharacterPool = new Dictionary<CharacterType, List<GameObject>>();

    private GameObject body;

    public override void Spawn()
    {
        CharacterPool.TryGetValue(Actual.Type, out List<GameObject> pool);
        Point3Int gridPosition = new Point3Int(
            Actual.GridPosition.x,
            Actual.GridPosition.y,
            Managers.World.World.GetTopHex(Actual.GridPosition.x, Actual.GridPosition.y).z);
        transform.position = WorldConversions.HexToUnityPosition(gridPosition);
        body = Helpers.GetFromPoolOrCreate(Models.GetCharacterModel(Actual.Type), pool, transform);
    }

    public override void Despawn()
    {
        body.SetActive(false);
        body.transform.SetParent(null);
        if (!CharacterPool.ContainsKey(Actual.Type))
            CharacterPool[Actual.Type] = new List<GameObject>();
        CharacterPool[Actual.Type].Add(body);

        Destroy(gameObject);
    }
}