using Core;
using UnityEngine;

public static class RaycastHelper
{
    public static Point3Int? GetHexUnderCursor()
    {
        var hit = Physics.Raycast(
            Managers.MainCamera.ScreenPointToRay(Input.mousePosition),
            out var hitInfo,
            100f,
            Layers.HexMask);
        if (hit)
        {
            Point2Int hex = (Point2Int)WorldConversions.UnityPositionToHex(hitInfo.point);
            return WorldMono.Instance.World.GetTopHex(hex);
        }
        else
        {
            return null;
        }
    }

    public class PointWithSide
    {
        public Point3Int Point;
        public HexSide hexSide;
    }
    private static PointWithSide cachedPointWithSide = new PointWithSide();
    public static PointWithSide? GetTriUnderCursor()
    {
        var hit = Physics.Raycast(
            Managers.MainCamera.ScreenPointToRay(Input.mousePosition),
            out var hitInfo,
            100f,
            Layers.HexMask);
        if (hit)
        {
            cachedPointWithSide.Point = WorldConversions.UnityPositionToHex(hitInfo.point);
            cachedPointWithSide.hexSide = GridHelpers.pixel_hex_side(new Point2Float(hitInfo.point.x, hitInfo.point.z));
            return cachedPointWithSide;
        }
        else
        {
            return null;
        }
    }

    public static Vector3? GetGroundPointUnderCursor()
    {
        var hit = Physics.Raycast(
            Managers.MainCamera.ScreenPointToRay(Input.mousePosition),
            out var hitInfo,
            100f,
            Layers.HexMask);
        if (hit)
        {
            return hitInfo.point;
        }
        else
        {
            return null;
        }
    }

    public static Interactable? GetInteractableUnderCursor()
    {
        var hit = Physics.Raycast(
            Managers.MainCamera.ScreenPointToRay(Input.mousePosition),
            out var hitInfo,
            100f,
            Layers.InteractableLayersMask,
            QueryTriggerInteraction.Collide);
        if (hit)
        {
            return FindInteractableInHierarchy(hitInfo.collider.gameObject);
        }
        else
        {
            return null;
        }
    }

    public static Interactable? FindInteractableInHierarchy(GameObject gameObject)
    {
        var iter = gameObject.transform;
        do
        {
            if (iter.gameObject.TryGetComponent<Interactable>(out Interactable interactable))
            {
                return interactable;
            }

            iter = iter.parent;
        }
        while (iter?.parent != null);

        return null;
    }
}