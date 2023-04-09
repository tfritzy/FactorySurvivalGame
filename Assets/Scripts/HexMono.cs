using System.Collections.Generic;
using Core;
using UnityEngine;

public class HexMono : EntityMono
{
    public Hex Actual;

    private static Dictionary<HexType, List<GameObject>> HexPool = new Dictionary<HexType, List<GameObject>>();

    private List<CharacterMono> characters = new List<CharacterMono>();
    private GameObject body;

    public override void Tick(float deltaTime)
    {
        foreach (CharacterMono characterMono in this.characters)
        {
            characterMono.Tick(deltaTime);
        }
    }

    public override void Spawn()
    {
        // Spawn own body
        HexPool.TryGetValue(this.Actual.Type, out List<GameObject> pool);
        this.transform.position = WorldConversions.HexToUnityPosition(this.Actual.GridPosition);
        this.body = Helpers.GetFromPoolOrCreate(Models.GetHexModel(this.Actual.Type), pool, this.transform);

        // Spawn building on top
        Point3Int pos = this.Actual.GridPosition;
        if (pos.z == Managers.World.World.GetTopHexHeight(pos.x, pos.y))
        {
            Building building = Managers.World.World.GetBuildingAt(pos.x, pos.y);
            if (building != null)
            {
                CharacterMono characterMono = GetCharacterMono(building.Type);
                characterMono.Actual = building;
                characterMono.Spawn();
                characters.Add(characterMono);
            }
        }
    }

    public override void Despawn()
    {
        // Despawn body
        this.body.SetActive(false);
        this.body.transform.SetParent(null);
        if (!HexPool.ContainsKey(HexType.Dirt))
            HexPool[HexType.Dirt] = new List<GameObject>();
        HexPool[HexType.Dirt].Add(this.body);

        // Despawn building on top
        foreach (CharacterMono characterMono in this.characters)
        {
            characterMono.Despawn();
        }

        Destroy(this.gameObject);
    }

    private static CharacterMono GetCharacterMono(CharacterType type)
    {
        GameObject shell = new GameObject();
        switch (type)
        {
            case CharacterType.Conveyor:
                return shell.AddComponent<ConveyorMono>();
            default:
                return null;
        }
    }
}