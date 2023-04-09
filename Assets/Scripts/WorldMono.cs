using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

public class WorldMono : MonoBehaviour
{
    public World World => Context.World;
    private Context Context;
    private RectInt ShownHexRange = new RectInt(-15, -8, 30, 24);
    private Dictionary<Point3Int, HexMono> ShownHexesObjects = new Dictionary<Point3Int, HexMono>();
    private Point2Int PlayerPos = new Point2Int(-1, -1);

    void Awake()
    {
        this.Context = new Context();
        TerrainGenerator generator = new TerrainGenerator(100, 100, 25);
        this.Context.World = new Core.World(generator.GenerateFlatWorld(this.Context));

        Conveyor first = new Conveyor(this.Context);
        this.Context.World.AddBuilding(first, new Point2Int(0, 0));
        this.Context.World.AddBuilding(new Conveyor(this.Context), new Point2Int(1, 1));
        this.Context.World.AddBuilding(new Conveyor(this.Context), new Point2Int(2, 1));
        this.Context.World.AddBuilding(new Conveyor(this.Context), new Point2Int(3, 2));
        this.Context.World.AddBuilding(new Conveyor(this.Context), new Point2Int(4, 2));
        this.Context.World.AddBuilding(new Conveyor(this.Context), new Point2Int(5, 3));
        this.Context.World.AddBuilding(new Conveyor(this.Context), new Point2Int(6, 3));
        this.Context.World.AddBuilding(new Conveyor(this.Context), new Point2Int(7, 4));

        first.ConveyorComponent.AddItem(new Stone());
    }

    void Update()
    {
        this.Context.World.Tick(Time.deltaTime);

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

                if (x < 0 || x >= this.Context.World.MaxX || y < 0 || y >= this.Context.World.MaxY)
                {
                    continue;
                }

                int topHex = this.Context.World.GetTopHexHeight(x, y);
                Point3Int point = new Point3Int(x, y, topHex);

                if (ShownHexesObjects.ContainsKey(point) || Context.World.GetHex(point) == null)
                {
                    continue;
                }

                GameObject shell = new GameObject("Hex");
                HexMono hex = shell.AddComponent<HexMono>();
                hex.Actual = Context.World.GetHex(point);
                ShownHexesObjects[point] = hex;
                hex.Spawn();
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
            HexMono hex = ShownHexesObjects[point];
            ShownHexesObjects.Remove(point);
            hex.Despawn();
        }
    }
}
