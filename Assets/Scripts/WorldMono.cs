using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

public class WorldMono : MonoBehaviour
{
    public World World => Context.World;
    public Context Context;
    private RectInt ShownHexRange = new RectInt(-15, -8, 30, 24);
    private Dictionary<Point3Int, GameObject[]> ShownHexesObjects = new Dictionary<Point3Int, GameObject[]>();
    private Point2Int PlayerPos = new Point2Int(-1, -1);
    private Conveyor first;

    void Awake()
    {
        Context = new Context();
        TerrainGenerator generator = new TerrainGenerator(100, 100, 25);
        Context.SetWorld(new World(new Core.Terrain(generator.GenerateFlatWorld(Context))));
    }

    void Update()
    {
        Context.World.Tick(Time.deltaTime);

        Point2Int currentPos = WorldConversions.UnityPositionToHex(Managers.Player.transform.position);
        if (currentPos != PlayerPos)
        {
            PlayerPos = currentPos;
            UpdateShownHex();
        }
    }

    private void UpdateShownHex()
    {
        DespawnOutOfRangeHex();
        for (int x = PlayerPos.x + ShownHexRange.min.x; x < PlayerPos.x + ShownHexRange.max.x; x++)
        {
            for (int y = PlayerPos.y + ShownHexRange.min.y; y < PlayerPos.y + ShownHexRange.max.y; y++)
            {
                if (x < 0 || x >= Context.World.MaxX || y < 0 || y >= Context.World.MaxY)
                {
                    continue;
                }

                Point3Int topHex = Context.World.GetTopHex(x, y);

                if (ShownHexesObjects.ContainsKey(topHex) || Context.World.Terrain.GetAt(topHex) == null)
                {
                    continue;
                }

                var point = Context.World.Terrain.GetAt(topHex);
                GameObject[] hex = new GameObject[6];
                for (int i = 0; i < 6; i++)
                {
                    hex[i] = Instantiate(Models.GetTriangleMesh(point.Value.Traingles[i].SubType));
                    hex[i].transform.position = WorldConversions.HexToUnityPosition(topHex);
                    hex[i].transform.SetParent(transform);
                    hex[i].transform.rotation = Quaternion.Euler(0, 60 * i, 0);
                }
                ShownHexesObjects.Add(topHex, hex);
            }
        }
    }

    private void DespawnOutOfRangeHex()
    {
        List<Point3Int> toRemove = new List<Point3Int>();
        foreach (Point3Int point in ShownHexesObjects.Keys)
        {
            if (point.x < PlayerPos.x + ShownHexRange.min.x || point.x > PlayerPos.x + ShownHexRange.max.x ||
                point.y < PlayerPos.y + ShownHexRange.min.y || point.y > PlayerPos.y + ShownHexRange.max.y)
            {
                toRemove.Add(point);
            }
        }

        foreach (Point3Int point in toRemove)
        {
            for (int i = 0; i < 6; i++)
            {
                Destroy(ShownHexesObjects[point][i]);
            }
        }

        foreach (Point3Int point in toRemove)
        {
            ShownHexesObjects.Remove(point);
        }
    }
}
