using System.Collections;
using System.Collections.Generic;
using FactoryCore;
using UnityEngine;

public class WorldMono : MonoBehaviour
{
    private World World;

    void Awake()
    {
        TerrainGenerator generator = new TerrainGenerator(100, 100, 25);
        this.World = new World(generator.GenerateFlatWorld());
        SpawnHex();
    }

    private void SpawnHex()
    {
        for (int x = 0; x < this.World.MaxX; x++)
        {
            for (int y = 0; y < this.World.MaxY; y++)
            {
                int topHex = this.World.GetTopHexHeight(x, y);
                GameObject.Instantiate(
                    Models.GetHexModel(HexModelType.DirtWithGrass),
                    WorldConversions.HexToUnityPosition(new Point3Int(x, y, topHex)),
                    Quaternion.identity,
                    this.transform);
            }
        }
    }
}
