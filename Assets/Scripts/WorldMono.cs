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
    private Dictionary<Point2Int, GameObject[]> ShownHexesObjects = new Dictionary<Point2Int, GameObject[]>();
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
            new Core.Terrain(generator.GenerateFlatWorld(Context),
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
                    var point2Hex = (Point2Int)hex;
                    if (ShownHexesObjects.ContainsKey(point2Hex) || Context.World.Terrain.GetAt(hex) == null)
                    {
                        continue;
                    }

                    var point = Context.World.Terrain.GetAt(hex);
                    GameObject[] hexes = new GameObject[6];
                    for (int i = 0; i < 6; i++)
                    {
                        hexes[i] = HexPool.GetTri(point[i].SubType, this.transform);
                        hexes[i].transform.position = WorldConversions.HexToUnityPosition(hex);
                        hexes[i].transform.SetParent(transform);
                        hexes[i].transform.rotation = Quaternion.Euler(0, 60 * i, 0);
                    }
                    ShownHexesObjects.Add(point2Hex, hexes);
                    if (World.GetBuildingAt(point2Hex) != null)
                    {
                        SetGrassActiveForHex(point2Hex, false);
                    }
                }
            }
        }
    }

    private void DespawnOutOfRangeHex()
    {
        List<Point2Int> toRemove = new();
        foreach (Point2Int point in ShownHexesObjects.Keys)
        {
            if (point.x < PlayerPos.x + ShownHexRange.min.x || point.x > PlayerPos.x + ShownHexRange.max.x ||
                point.y < PlayerPos.y + ShownHexRange.min.y || point.y > PlayerPos.y + ShownHexRange.max.y)
            {
                toRemove.Add(point);
            }
        }

        foreach (Point2Int point in toRemove)
        {
            SetGrassActiveForHex(point, true);
            for (int i = 0; i < 6; i++)
            {
                var parsedType =
                    System.Enum.Parse(typeof(TriangleSubType), ShownHexesObjects[point][i].name);
                HexPool.ReturnTri((TriangleSubType)parsedType, ShownHexesObjects[point][i]);
            }
        }

        foreach (Point2Int point in toRemove)
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
                SetGrassActiveForHex((Point2Int)newBuilding.GridPosition, false);
                break;
            case UpdateType.BuildingRemoved:
                BuildingRemoved updateBuildingRemoved = (BuildingRemoved)update;
                Destroy(SpawnedCharacters[updateBuildingRemoved.Id]);
                SpawnedCharacters.Remove(updateBuildingRemoved.Id);
                SetGrassActiveForHex((Point2Int)updateBuildingRemoved.GridPosition, true);
                break;
        }
        World.AckUpdate();
    }

    private void SetGrassActiveForHex(Point2Int hex, bool active)
    {
        if (ShownHexesObjects.ContainsKey(hex))
        {
            for (int i = 0; i < 6; i++)
            {
                ShownHexesObjects[hex][i].transform.Find("Grass").gameObject.SetActive(active);
            }
        }
    }
}
