using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FactoryCore;
using UnityEngine;

public class WorldMono : MonoBehaviour
{
    private World World;
    private RectInt ShownHexRange = new RectInt(-15, -8, 30, 24);
    private Dictionary<Point3Int, GameObject> ShownHexesObjects = new Dictionary<Point3Int, GameObject>();
    private Point2Int LastPlayerPosition = new Point2Int(-1, -1);
    private Dictionary<PrefabType, List<GameObject>> PrefabPool = new Dictionary<PrefabType, List<GameObject>>();

    void Awake()
    {
        Context context = new Context();
        TerrainGenerator generator = new TerrainGenerator(100, 100, 25);
        context.World = new World(generator.GenerateFlatWorld(context));

        Conveyor first = new Conveyor(context);
        this.World.AddBuilding(first, new Point2Int(0, 0));
        this.World.AddBuilding(new Conveyor(context), new Point2Int(1, 1));
        this.World.AddBuilding(new Conveyor(context), new Point2Int(2, 1));
        this.World.AddBuilding(new Conveyor(context), new Point2Int(3, 2));
        this.World.AddBuilding(new Conveyor(context), new Point2Int(4, 2));
        this.World.AddBuilding(new Conveyor(context), new Point2Int(5, 3));
        this.World.AddBuilding(new Conveyor(context), new Point2Int(6, 3));
        this.World.AddBuilding(new Conveyor(context), new Point2Int(7, 4));

        first.Cell.AddItem(new Stone());
    }

    void Update()
    {
        UpdateShownHex();
        World.Tick(Time.deltaTime);
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

                GameObject hex = GetFromPoolOrCreate(PrefabType.Hex_DirtWithGrass, point);
                ShownHexesObjects[point] = hex;
            }
        }
    }

    private GameObject GetFromPoolOrCreate(PrefabType type, Point3Int point)
    {
        if (PrefabPool.ContainsKey(type) && PrefabPool[type].Count > 0)
        {
            GameObject poolObj = PrefabPool[type].Last();
            poolObj.SetActive(true);
            PrefabPool[type].RemoveAt(PrefabPool[type].Count - 1);
            poolObj.transform.position = WorldConversions.HexToUnityPosition(point);
            return poolObj;
        }
        else
        {
            return GameObject.Instantiate(
                Models.GetHexModel(type),
                WorldConversions.HexToUnityPosition(point),
                Quaternion.identity,
                this.transform
            );
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
            hex.SetActive(false);

            if (!PrefabPool.ContainsKey(PrefabType.Hex_DirtWithGrass))
            {
                PrefabPool[PrefabType.Hex_DirtWithGrass] = new List<GameObject>();
            }

            PrefabPool[PrefabType.Hex_DirtWithGrass].Add(hex);
        }
    }
}
