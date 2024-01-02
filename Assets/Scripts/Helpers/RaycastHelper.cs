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

    public static Interactable? GetInteractableUnderCursor()
    {
        var hit = Physics.Raycast(
            Managers.MainCamera.ScreenPointToRay(Input.mousePosition),
            out var hitInfo,
            100f,
            Layers.CharacterMask | Layers.VegetationMask);
        if (hit)
        {
            var iter = hitInfo.collider.transform;
            while (iter.parent != null)
            {
                if (iter.gameObject.TryGetComponent<CharacterMono>(out var cm))
                {
                    return cm;
                }

                iter = iter.parent;
            }

            return null;
        }
        else
        {
            return null;
        }
    }
}