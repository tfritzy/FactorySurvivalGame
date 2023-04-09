using System.Collections.Generic;
using Core;
using UnityEngine;

public class HexMono : EntityMono
{
    public Hex Hex => (Hex)this.Entity;

    private static Dictionary<HexType, List<GameObject>> HexPool = new Dictionary<HexType, List<GameObject>>();

    private List<CharacterMono> characters = new List<CharacterMono>();
    private GameObject body;

    public override void Spawn()
    {
        Debug.Log("Spawning hex at " + this.Hex.GridPosition);
        Debug.Log("My hex is " + this.Hex?.Type);

        // Spawn own body
        HexPool.TryGetValue(Hex.Type, out List<GameObject> pool);
        Debug.Log("Model" + Models.GetHexModel(Hex.Type));
        Debug.Log("Pool" + pool);
        Debug.Log("Pos" + this.Hex.GridPosition);
        Debug.Log("Parent" + this.transform);
        this.body = Helpers.GetFromPoolOrCreate(Models.GetHexModel(Hex.Type), pool, this.Hex.GridPosition, this.transform);

        // Spawn building on top
        Point3Int pos = this.Hex.GridPosition;
        if (pos.z == Managers.World.World.GetTopHexHeight(pos.x, pos.y))
        {
            Building building = Managers.World.World.GetBuildingAt(pos.x, pos.y);
            if (building != null)
            {
                CharacterMono characterMono = GetCharacterMono(building.Type);
                characterMono.Entity = building;
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