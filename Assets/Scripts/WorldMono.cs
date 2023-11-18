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
    private Dictionary<ulong, GameObject> SpawnedCharacters = new Dictionary<ulong, GameObject>();
    private Point2Int PlayerPos = new Point2Int(-1, -1);
    private Conveyor first;

    private static WorldMono instance;
    public static WorldMono Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<WorldMono>();
            }
            return instance;
        }
    }

    void Awake()
    {
        Context = new Context();
        TerrainGenerator generator = new TerrainGenerator(100, 100, 25);
        Context.SetWorld(new World(
            new Core.Terrain(generator.GenerateRollingHills(Context),
            Context)));
    }

    void Update()
    {
        Context.World.Tick(Time.deltaTime);

        Point2Int currentPos = (Point2Int)WorldConversions.UnityPositionToHex(Managers.Player.transform.position);
        if (currentPos != PlayerPos)
        {
            PlayerPos = currentPos;
            UpdateShownHex();
        }

        while (World.UnseenUpdates.Count > 0)
        {
            HandleUpdate(World.UnseenUpdates.First());
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

                int topHeight = Context.World.Terrain.GetTopHex(new Point2Int(x, y)).z;
                for (int z = topHeight; z >= 0; z--)
                {
                    Point3Int hex = new Point3Int(x, y, z);
                    if (ShownHexesObjects.ContainsKey(hex) || Context.World.Terrain.GetAt(hex) == null)
                    {
                        continue;
                    }

                    var point = Context.World.Terrain.GetAt(hex);
                    GameObject[] hexes = new GameObject[6];
                    for (int i = 0; i < 6; i++)
                    {
                        hexes[i] = Instantiate(Models.GetTriangleMesh(point[i].SubType));
                        hexes[i].transform.position = WorldConversions.HexToUnityPosition(hex);
                        hexes[i].transform.SetParent(transform);
                        hexes[i].transform.rotation = Quaternion.Euler(90, 60 * i, 0);
                    }
                    ShownHexesObjects.Add(hex, hexes);
                }
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

    private void HandleUpdate(Update update)
    {
        Debug.Log("Handling event of type: " + update.Type);
        switch (update.Type)
        {
            case UpdateType.BuildingAdded:
                BuildingAdded updateBuildingAdded = (BuildingAdded)update;
                Building newBuilding = World.GetBuildingAt(updateBuildingAdded.GridPosition);
                GameObject building = Instantiate(Models.GetCharacterModel(newBuilding.Type));
                building.transform.position = WorldConversions.HexToUnityPosition(newBuilding.GridPosition);
                building.transform.SetParent(transform);
                SpawnedCharacters.Add(newBuilding.Id, building);
                building.GetComponent<EntityMono>().Setup(newBuilding);
                break;
            case UpdateType.BuildingRemoved:
                BuildingRemoved updateBuildingRemoved = (BuildingRemoved)update;
                Destroy(SpawnedCharacters[updateBuildingRemoved.Id]);
                SpawnedCharacters.Remove(updateBuildingRemoved.Id);
                break;
        }
        World.AckUpdate();
    }
}
