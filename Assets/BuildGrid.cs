using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

public class BuildGrid : MonoBehaviour
{
    GameObject BuildGridPrefab;
    GameObject BuildGridFilledPrefab;
    private static readonly Color BuildableColor = ColorExtensions.FromHex("#19c512");
    private static readonly Color NeutralColor = ColorExtensions.FromHex("#CCCCCC");
    private static readonly Color DistanceOne = ColorExtensions.FromHex("#CCCCCCAA");
    private static readonly Color DistanceTwo = ColorExtensions.FromHex("#CCCCCC66");
    private static readonly Color UnbuildableColor = ColorExtensions.FromHex("#FFBA3C");

    private List<GameObject> RentedFilled = new();
    private List<GameObject> RentedEmpty = new();
    private static List<GameObject> FilledPool = new();
    private static List<GameObject> EmptyPool = new();

    void Awake()
    {
        transform.position = WorldConversions.HexToUnityPosition(Point3Int.Zero);
        BuildGridPrefab = Resources.Load<GameObject>("Prefabs/Decals/BuildGridHex");
        BuildGridFilledPrefab = Resources.Load<GameObject>("Prefabs/Decals/BuildGridHexFilled");
    }

    public void SetPos(Point3Int pos)
    {
        ReturnAllRented();

        List<Point3Int> ring1 =
            GridHelpers
                .GetHexRing((Point2Int)pos, 1)
                .Select((pos) => WorldMono.Instance.World.GetTopHex(pos))
                .ToList();
        List<Point3Int> ring2 =
            GridHelpers
                .GetHexRing((Point2Int)pos, 2)
                .Select((pos) => WorldMono.Instance.World.GetTopHex(pos))
                .ToList();

        ConfigureHex(pos, 0, true);

        foreach (Point3Int hex in ring1)
        {
            ConfigureHex(hex, 1, false);
        }

        foreach (Point3Int hex in ring2)
        {
            ConfigureHex(hex, 2, false);
        }
    }

    private Color getNeutralColor(int distance)
    {
        if (distance == 0)
        {
            return NeutralColor;
        }
        else if (distance == 1)
        {
            return DistanceOne;
        }
        else
        {
            return DistanceTwo;
        }
    }

    private void ConfigureHex(Point3Int pos, int distance, bool isOrigin)
    {
        GameObject hexObj;
        bool isBuildable = WorldMono.Instance.World.GetBuildingAt((Point2Int)pos) == null;
        if (!isOrigin)
        {
            hexObj = !isBuildable ? GetFilled() : GetEmpty();
            hexObj.GetComponent<Renderer>().material.color = isBuildable ? getNeutralColor(distance) : UnbuildableColor;
        }
        else
        {
            hexObj = GetFilled();
            hexObj.GetComponent<Renderer>().material.color = isBuildable ? BuildableColor : UnbuildableColor;
        }

        hexObj.transform.position = WorldConversions.HexToUnityPosition(pos);
    }

    private GameObject GetFilled()
    {
        if (FilledPool.Count == 0)
        {
            FilledPool.Add(Instantiate(BuildGridFilledPrefab, transform));
        }

        var filled = FilledPool.Last();
        FilledPool.RemoveAt(FilledPool.Count - 1);
        RentedFilled.Add(filled);
        filled.SetActive(true);
        return filled;
    }

    private GameObject GetEmpty()
    {
        if (EmptyPool.Count == 0)
        {
            EmptyPool.Add(Instantiate(BuildGridPrefab, transform));
        }

        var empty = EmptyPool.Last();
        EmptyPool.RemoveAt(EmptyPool.Count - 1);
        RentedEmpty.Add(empty);
        empty.SetActive(true);
        return empty;
    }

    private void ReturnAllRented()
    {
        foreach (var filled in RentedFilled)
        {
            filled.SetActive(false);
            FilledPool.Add(filled);
        }

        foreach (var empty in RentedEmpty)
        {
            empty.SetActive(false);
            EmptyPool.Add(empty);
        }

        RentedFilled.Clear();
        RentedEmpty.Clear();
    }
}
