using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Core;
using Google.Protobuf;
using HighlightPlus;
using UnityEngine;

public class WorldMono : MonoBehaviour
{
    public World World => Context.World;
    public Context Context;
    private RectInt ShownHexRange = new RectInt(-15, -5, 30, 23);
    private Dictionary<Point2Int, Dictionary<Point3Int, GameObject?[]>> ShownHexesObjects = new();
    private Dictionary<ulong, GameObject> SpawnedCharacters = new();
    private Point2Int PlayerPos = new Point2Int(-1, -1);
    private Dictionary<Point2Int, GameObject> SpawnedVegetation = new();
    private Dictionary<ulong, GameObject> SpawnedItemObjects = new();
    Transform? vegetationParent;

    [Range(.25f, 4f)]
    public float TickPercent = 1;

    private static WorldMono instance;
    public static WorldMono Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<WorldMono>();
            }
            return instance;
        }
    }

    void Awake()
    {
        Application.targetFrameRate = 60;
        Context = new Context();
    }

    public void SetWorld(World world)
    {
        Context.SetWorld(world);
        World.AddCharacter(PlayerMono.Instance.Actual);
        SpawnVegetation();
    }

    void Update()
    {
        if (!Context.HasWorld)
        {
            return;
        }

        Context.World.Tick(Time.deltaTime * TickPercent);

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
        if (!Context.HasWorld)
        {
            return;
        }

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
                var point2Hex = new Point2Int(x, y);
                if (ShownHexesObjects.ContainsKey(point2Hex))
                {
                    continue;
                }
                ShownHexesObjects.Add(point2Hex, new Dictionary<Point3Int, GameObject[]>());

                for (int z = topHeight; z >= 0; z--)
                {
                    Point3Int hex = new Point3Int(x, y, z);
                    if (Context.World.Terrain.GetAt(hex) == null)
                    {
                        continue;
                    }

                    Schema.Triangle?[]? point = Context.World.Terrain.GetAt(hex);
                    GameObject[] hexes = new GameObject[6];
                    bool allFull = point.All(p => p.SubType == TriangleSubType.LandFull);
                    ShownHexesObjects[point2Hex][hex] = new GameObject[6];
                    if (allFull)
                    {
                        hexes[0] = HexPool.GetTri(TriangleSubType.LandActuallyFull, this.transform);
                        hexes[0].transform.position = WorldConversions.HexToUnityPosition(hex);
                        hexes[0].transform.SetParent(transform);
                        ShownHexesObjects[point2Hex][hex][0] = hexes[0];
                    }
                    else
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            hexes[i] = HexPool.GetTri(point[i].SubType, this.transform);
                            hexes[i].transform.position = WorldConversions.HexToUnityPosition(hex);
                            hexes[i].transform.SetParent(transform);
                            hexes[i].transform.rotation = Quaternion.Euler(0, 60 * i, 0);
                            ShownHexesObjects[point2Hex][hex][i] = hexes[i];
                        }
                    }

                    if (z == topHeight && World.GetBuildingAt(point2Hex) == null)
                    {
                        SetGrassActiveForHex(point2Hex, true);
                    }
                }
            }
        }
    }

    private void DespawnOutOfRangeHex()
    {
        if (!Context.HasWorld)
        {
            return;
        }

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
            SetGrassActiveForHex(point, false);
            foreach (GameObject[] hex in ShownHexesObjects[point].Values)
            {
                foreach (GameObject tri in hex)
                {
                    if (tri == null)
                    {
                        continue;
                    }

                    var parsedType =
                        System.Enum.Parse(typeof(TriangleSubType), tri.name);
                    HexPool.ReturnTri((TriangleSubType)parsedType, tri);
                }
            }
        }

        foreach (Point2Int point in toRemove)
        {
            ShownHexesObjects.Remove(point);
        }
    }

    private void SpawnVegetation()
    {
        Transform vegetationParent = new GameObject("Vegetation").transform;
        vegetationParent.transform.SetParent(transform);

        for (int x = 0; x < Context.World.MaxX; x++)
        {
            for (int y = 0; y < Context.World.MaxY; y++)
            {
                TerrainObject? obj = Context.World.Terrain.TerrainObjects[x, y];
                if (obj != null)
                {
                    SpawnSingleVegetation(new Point2Int(x, y), obj.Type);
                }
            }
        }
    }

    private void SpawnSingleVegetation(Point2Int pos, TerrainObjectType? type)
    {
        if (SpawnedVegetation.ContainsKey(pos))
        {
            Destroy(SpawnedVegetation[pos]);
        }

        if (type == null)
        {
            return;
        }

        GameObject vegetation =
            VegetationPool.GetVegetation(type.Value, vegetationParent);
        Point3Int topPoint = Context.World.Terrain.GetTopHex(pos);
        vegetation.transform.position = WorldConversions.HexToUnityPosition(topPoint);
        vegetation.transform.SetParent(transform);
        vegetation.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        vegetation.transform.localScale = vegetation.transform.localScale * Random.Range(.8f, 1.2f);
        vegetation.layer = Layers.Vegetation;
        var vm = vegetation.AddComponent<VegetationMono>();
        vm.Init(pos, type.Value);
        var he = vegetation.AddComponent<HighlightEffect>();
        he.ProfileLoad(
            HighlightProfiles.GetHighlightProfile(
                HighlightProfiles.Profile.Highlighted));
        SpawnedVegetation[pos] = vegetation;
    }

    private void HandleUpdate(Schema.OneofUpdate update)
    {
        switch (update.UpdateCase)
        {
            case Schema.OneofUpdate.UpdateOneofCase.BuildingAdded:
                Schema.BuildingAdded updateBuildingAdded = update.BuildingAdded;
                Building newBuilding = World.GetBuildingAt(Point2Int.FromSchema(updateBuildingAdded.GridPosition));
                GameObject building = Instantiate(Models.GetCharacterModel(newBuilding.Type));
                building.transform.position = WorldConversions.HexToUnityPosition(newBuilding.GridPosition);
                building.transform.SetParent(transform);
                SpawnedCharacters.Add(newBuilding.Id, building);
                var em = building.GetComponent<EntityMono>();
                em.Setup(newBuilding);
                SetGrassActiveForHex((Point2Int)newBuilding.GridPosition, false);
                break;
            case Schema.OneofUpdate.UpdateOneofCase.BuildingRemoved:
                Schema.BuildingRemoved updateBuildingRemoved = update.BuildingRemoved;
                Destroy(SpawnedCharacters[updateBuildingRemoved.BuildingId]);
                SpawnedCharacters.Remove(updateBuildingRemoved.BuildingId);
                SetGrassActiveForHex((Point2Int)Point3Int.FromSchema(updateBuildingRemoved.GridPosition), true);
                break;
                // case UpdateType.TriUncoveredOrAdded:
                //     HandleTriUncoveredOrAdded((TriUncoveredOrAdded)update);
                //     break;
                // case UpdateType.TriHiddenOrDestroyed:
                //     HandleTriHiddenOrDestroyed((TriHiddenOrDestroyed)update);
                //     break;
                // case UpdateType.TerrainObjectChange:
                //     HandleTerrainObjectChange((TerrainObjectChange)update);
                //     break;
                // case UpdateType.ItemObjectAdded:
                //     HandleItemObjectAdded((ItemObjectAdded)update);
                //     break;
                // case UpdateType.ItemObjectRemoved:
                //     HandleItemObjectRemoved((ItemObjectRemoved)update);
                //     break;
        }
        World.AckUpdate();
    }

    // private void HandleTriUncoveredOrAdded(TriUncoveredOrAdded update)
    // {
    //     Schema.Triangle? triangle = World.Terrain.GetTri(update.GridPosition, update.Side);
    //     if (triangle != null)
    //     {
    //         var triGO = GameObject.Instantiate(Models.GetTriangleMesh(triangle.SubType), transform);
    //         triGO.transform.position = WorldConversions.HexToUnityPosition(update.GridPosition);
    //         triGO.transform.rotation = Quaternion.Euler(0, 60 * (int)update.Side, 0);
    //     }
    // }

    private void RemoveLeavesFromBush(GameObject bushGameObject)
    {
        foreach (MeshRenderer renderer in bushGameObject.GetComponentsInChildren<MeshRenderer>())
        {
            renderer.material = null;
        }
    }

    // private void HandleTerrainObjectChange(TerrainObjectChange vegetationChange)
    // {
    //     if (vegetationChange.NewVegeType == TerrainObjectType.StrippedBush
    //         && SpawnedVegetation.ContainsKey(vegetationChange.GridPosition))
    //     {
    //         RemoveLeavesFromBush(SpawnedVegetation[vegetationChange.GridPosition]);
    //     }
    //     else
    //     {
    //         SpawnSingleVegetation(vegetationChange.GridPosition, vegetationChange.NewVegeType);
    //     }
    // }

    // private void HandleTriHiddenOrDestroyed(TriHiddenOrDestroyed update)
    // {
    //     if (ShownHexesObjects.ContainsKey((Point2Int)update.GridPosition))
    //     {
    //         if (ShownHexesObjects[(Point2Int)update.GridPosition].ContainsKey((Point3Int)update.GridPosition))
    //         {
    //             var triGO = ShownHexesObjects[(Point2Int)update.GridPosition][(Point3Int)update.GridPosition][(int)update.Side];
    //             if (triGO != null)
    //             {
    //                 var parsedType =
    //                     System.Enum.Parse(typeof(TriangleSubType), triGO.name);
    //                 HexPool.ReturnTri((TriangleSubType)parsedType, triGO);
    //                 ShownHexesObjects
    //                     [(Point2Int)update.GridPosition]
    //                     [(Point3Int)update.GridPosition]
    //                     [(int)update.Side] = null;
    //             }
    //         }
    //     }
    // }

    // private void HandleItemObjectAdded(ItemObjectAdded objectAdded)
    // {
    //     GameObject itemObject = ItemPool.GetItem(objectAdded.ItemObject.Item.Type, this.transform);
    //     itemObject.transform.position = objectAdded.ItemObject.Position.ToVector3();
    //     itemObject.GetComponent<ItemMono>().SetItem(Item.FromSchema(objectAdded.ItemObject.Item));
    //     SpawnedItemObjects.Add(objectAdded.ItemObject.Item.Id, itemObject);
    // }

    // private void HandleItemObjectRemoved(ItemObjectRemoved objectRemoved)
    // {
    //     if (SpawnedItemObjects.ContainsKey(objectRemoved.ItemId))
    //     {
    //         Destroy(SpawnedItemObjects[objectRemoved.ItemId]);
    //         SpawnedItemObjects.Remove(objectRemoved.ItemId);
    //     }
    // }

    private void SetGrassActiveForHex(Point2Int hex, bool active)
    {
        // if (ShownHexesObjects.ContainsKey(hex))
        // {
        //     foreach (var tri in ShownHexesObjects[hex])
        //     {
        //         tri.transform.Find("Grass")?.gameObject.SetActive(active);
        //     }
        // }
    }
}
