using System.Collections.Generic;
using Core;
using UnityEngine;

public abstract class CharacterMono : EntityMono
{
    public Character Character => (Character)this.Entity;

    private static Dictionary<CharacterType, List<GameObject>> CharacterPool = new Dictionary<CharacterType, List<GameObject>>();

    private GameObject body;

    public override void Spawn()
    {
        CharacterPool.TryGetValue(this.Character.Type, out List<GameObject> pool);
        Point3Int pos = new Point3Int(
            this.Character.GridPosition.x,
            this.Character.GridPosition.y,
            Managers.World.World.GetTopHexHeight(
                this.Character.GridPosition.x,
                this.Character.GridPosition.y));
        this.body = Helpers.GetFromPoolOrCreate(Models.GetCharacterModel(Character.Type), pool, pos, this.transform);
    }

    public override void Despawn()
    {
        this.body.SetActive(false);
        this.body.transform.SetParent(null);
        if (!CharacterPool.ContainsKey(Character.Type))
            CharacterPool[Character.Type] = new List<GameObject>();
        CharacterPool[Character.Type].Add(this.body);

        Destroy(this.gameObject);
    }
}