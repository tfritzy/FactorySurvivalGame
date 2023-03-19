using System.Collections;
using System.Collections.Generic;
using FactoryCore;
using UnityEngine;

public class WorldMono : MonoBehaviour
{
    private World World;
    private RectInt ShownHexRange = new RectInt(-15, -8, 30, 24);
    private Dictionary<Point3Int, GameObject> ShownHexesObjects = new Dictionary<Point3Int, GameObject>();
    private LinkedList<GameObject> HexPool = new LinkedList<GameObject>();
    private Point2Int LastPlayerPosition = new Point2Int(-1, -1);

    void Awake()
    {
        TerrainGenerator generator = new TerrainGenerator(100, 100, 25);
        this.World = new World(generator.GenerateFlatWorld());
    }

    void Update()
    {
        UpdateShownHex();
    }

    private void UpdateShownHex()
    {
        Point2Int currentPos = WorldConversions.UnityPositionToHex(Managers.Player.transform.position);
        if (currentPos == LastPlayerPosition)
        {
            return;
        }

        DespawnOutOfRangeHex(currentPos);

        LastPlayerPosition = currentPos;
        for (int x = currentPos.x + ShownHexRange.min.x; x < currentPos.x + ShownHexRange.max.x; x++)
        {
            for (int y = currentPos.y + ShownHexRange.min.y; y < currentPos.y + ShownHexRange.max.y; y++)
            {

                if (x < 0 || x >= this.World.MaxX || y < 0 || y >= this.World.MaxY)
                {
                    continue;
                }

                int topHex = this.World.GetTopHexHeight(x, y);
                Point3Int point = new Point3Int(x, y, topHex);

                if (ShownHexesObjects.ContainsKey(point))
                {
                    continue;
                }

                if (HexPool.Count > 0)
                {
                    GameObject pooledHex = HexPool.First.Value;
                    pooledHex.SetActive(true);
                    HexPool.RemoveFirst();
                    pooledHex.transform.position = WorldConversions.HexToUnityPosition(point);
                    ShownHexesObjects.Add(point, pooledHex);
                }
                else
                {
                    ShownHexesObjects.Add(
                        point,
                        GameObject.Instantiate(
                            Models.GetHexModel(HexModelType.DirtWithGrass),
                            WorldConversions.HexToUnityPosition(point),
                            Quaternion.identity,
                            this.transform
                        )
                    );
                }
            }
        }
    }

    private void DespawnOutOfRangeHex(Point2Int currentPos)
    {
        List<Point3Int> toRemove = new List<Point3Int>();
        foreach (Point3Int point in ShownHexesObjects.Keys)
        {
            if (point.x < currentPos.x + ShownHexRange.min.x || point.x > currentPos.x + ShownHexRange.max.x ||
                point.y < currentPos.y + ShownHexRange.min.y || point.y > currentPos.y + ShownHexRange.max.y)
            {
                toRemove.Add(point);
            }
        }

        foreach (Point3Int point in toRemove)
        {
            GameObject hex = ShownHexesObjects[point];
            ShownHexesObjects.Remove(point);
            HexPool.AddLast(hex);
            hex.SetActive(false);
        }
    }
}
