using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildGrid : MonoBehaviour
{
    private static readonly Color BuildableColor = ColorExtensions.FromHex("#000000AA");
    private static readonly Color NeutralColor = ColorExtensions.FromHex("#000000AA");
    private static readonly Color DistanceOne = ColorExtensions.FromHex("#000000AA");
    private static readonly Color DistanceTwo = ColorExtensions.FromHex("#000000AA");
    private static readonly Color UnbuildableColor = ColorExtensions.FromHex("#000000AA");

    private Transform? gridParent;
    private List<GameObject> RentedFilled = new();
    private List<GameObject> RentedEmpty = new();
    private static List<GameObject> FilledPool = new();
    private static List<GameObject> EmptyPool = new();

    private GameObject? previewBlock;
    private Building? previewBuilding;
    private HexSide rotation = 0;
    private Point3Int position;

    void Start()
    {
        gridParent = new GameObject("BuildGrid").transform;
        gridParent.SetParent(transform);
    }

    void Update()
    {
        if (
            PlayerMono.Instance?.SelectedItem?.Places == null &&
            PlayerMono.Instance?.SelectedItem?.Builds == null)
        {
            ClearPreviews();
            Disable();
            return;
        }

        RaycastHelper.PointWithSide? hex = RaycastHelper.GetTriUnderCursor();
        if (hex != null)
        {
            Point3Int topHex = WorldMono.Instance.World.GetTopHex(hex.Point.x, hex.Point.y, rotation);
            SetPos(topHex);
            SetItem(PlayerMono.Instance.SelectedItem, topHex);
            Enable();
        }

        PlaceBlock();
        MakePreviewBuildingReal();
        Rotate();
    }

    private void Disable()
    {
        previewBlock?.SetActive(false);
        gridParent?.gameObject.SetActive(false);
    }

    private void Enable()
    {
        previewBlock?.SetActive(true);
        gridParent?.gameObject.SetActive(true);
    }

    public void SetPos(Point3Int pos)
    {
        position = pos;
        transform.position = WorldConversions.HexToUnityPosition(pos);
        ReturnAllRented();

        List<Point2Int> ring1 =
            GridHelpers
                .GetHexRing(Point2Int.Zero, 1)
                .ToList();
        List<Point2Int> ring2 =
            GridHelpers
                .GetHexRing(Point2Int.Zero, 2)
                .ToList();

        ConfigureHex(Point2Int.Zero, 0, true);

        foreach (Point2Int hex in ring1)
        {
            ConfigureHex(hex, 1, false);
        }

        foreach (Point2Int hex in ring2)
        {
            ConfigureHex(hex, 2, false);
        }
    }

    private void Rotate()
    {
        if (Input.GetKeyDown(KeyCode.R) || Input.mouseScrollDelta.y > 0)
        {
            rotation = GridHelpers.Rotate60(rotation);
            SetRotation(rotation);
            previewBuilding?.SetRotation(rotation);
            // UpdateArrows();
        }
        else if (Input.GetKeyDown(KeyCode.E) || Input.mouseScrollDelta.y < 0)
        {
            rotation = GridHelpers.Rotate60(rotation, clockwise: false);
            SetRotation(rotation);
            previewBuilding?.SetRotation(rotation);
            // UpdateArrows();
        }
    }

    public void SetRotation(HexSide rotation)
    {
        this.rotation = rotation;
        if (previewBlock != null)
        {
            previewBlock.transform.localRotation = Quaternion.Euler(0, 60 * (int)rotation, 0);
        }
    }

    private ItemType? previewedItemType;
    public void SetItem(Item item, Point3Int pos)
    {
        if (item.Places != null && item.Type != previewedItemType)
        {
            PreviewBlock(item.Places);
            previewedItemType = item.Type;
        }

        if (item.Builds != null &&
            (previewBuilding?.GridPosition != pos || item.Type != previewedItemType))
        {
            PreviewBuilding(pos, item.Builds.Value);
            previewedItemType = item.Type;
        }
    }

    private void PreviewBuilding(Point3Int pos, CharacterType buildingType)
    {
        ClearPreviews();

        var building = PlayerMono.Instance.Actual.BuidPreviewBuildingFromItem(
            PlayerMono.Instance.SelectedInventoryIndex,
            (Point2Int)pos);
        if (building != null)
        {
            var actualBuilding = WorldMono.Instance.World.GetBuildingAt((Point2Int)pos);
            actualBuilding?.SetRotation(rotation);
            previewBuilding = actualBuilding;
        }
    }

    private void MakePreviewBuildingReal()
    {
        if (previewBuilding == null)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            PlayerMono.Instance.Actual.MakePreviewBuildingRealFromItem(
                PlayerMono.Instance.SelectedInventoryIndex,
                previewBuilding);
            previewBuilding = null;
            Disable();
        }
    }

    private void PreviewBlock(Item.PlacedTriangleMetadata[] places)
    {
        ClearPreviews();

        previewBlock = new GameObject("PreviewBlock");
        previewBlock.transform.SetParent(transform);
        previewBlock.transform.localPosition = Vector3.zero + Vector3.up * Constants.HEX_HEIGHT;
        previewBlock.transform.localRotation = Quaternion.Euler(0, 60 * (int)rotation, 0);

        List<Point3Int> locations = new List<Point3Int>();
        List<HexSide> subIndices = new List<HexSide>();
        foreach (Item.PlacedTriangleMetadata placed in places)
        {
            Point3Int placeLocation = Point3Int.Zero;
            foreach (HexSide posOffset in placed.PositionOffset)
            {
                placeLocation = GridHelpers.GetNeighbor(placeLocation, posOffset);
            }
            var rotatedSubIndex = placed.RotationOffset;

            locations.Add(placeLocation);
            subIndices.Add(rotatedSubIndex);
        }

        Vector3 origin = WorldConversions.HexToUnityPosition(Point3Int.Zero);
        for (int i = 0; i < locations.Count; i++)
        {
            Point3Int placeLocation = locations[i];
            HexSide rotatedSubIndex = subIndices[i];

            GameObject triMesh = HexPool.GetTri(places[i].Triangle.SubType, previewBlock.transform);
            triMesh.transform.localPosition = WorldConversions.HexToUnityPosition(placeLocation) - origin;
            triMesh.transform.localRotation = Quaternion.Euler(0, 60 * (int)rotatedSubIndex, 0);
        }
    }

    private void ClearPreviews()
    {
        if (previewBlock != null)
        {
            Destroy(previewBlock);
            previewBlock = null;
        }

        if (previewBuilding != null)
        {
            WorldMono.Instance.World.RemoveBuilding(previewBuilding.Id);
            previewBuilding = null;
        }
    }

    private void PlaceBlock()
    {
        if (PlayerMono.Instance.SelectedItem?.Places == null)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            PlayerMono.Instance.Actual.PlaceBlockFromItem(
                PlayerMono.Instance.SelectedInventoryIndex, position + Point3Int.Up, rotation);
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

    private void ConfigureHex(Point2Int pos, int distance, bool isOrigin)
    {
        GameObject hexObj;
        bool isBuildable = true;
        if (!isOrigin)
        {
            hexObj = GetEmpty();
            hexObj.GetComponent<Renderer>().material.color = isBuildable ? getNeutralColor(distance) : UnbuildableColor;
        }
        else
        {
            hexObj = GetFilled();
            hexObj.GetComponent<Renderer>().material.color = isBuildable ? BuildableColor : UnbuildableColor;
        }

        hexObj.transform.localPosition = WorldConversions.HexToUnityPosition((Point3Int)pos);
    }

    private GameObject GetFilled()
    {
        if (FilledPool.Count == 0)
        {
            FilledPool.Add(Instantiate(DecalLoader.GetDecalPrefab(DecalLoader.Decal.BuildGridHexFilled), transform));
        }

        var filled = FilledPool.Last();
        FilledPool.RemoveAt(FilledPool.Count - 1);
        RentedFilled.Add(filled);
        filled.SetActive(true);
        filled.transform.SetParent(gridParent);
        return filled;
    }

    private GameObject GetEmpty()
    {
        if (EmptyPool.Count == 0)
        {
            EmptyPool.Add(Instantiate(DecalLoader.GetDecalPrefab(DecalLoader.Decal.BuildGridHex), transform));
        }

        var empty = EmptyPool.Last();
        EmptyPool.RemoveAt(EmptyPool.Count - 1);
        RentedEmpty.Add(empty);
        empty.SetActive(true);
        empty.transform.SetParent(gridParent);
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
