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

    public static CharacterMono? GetCharacterUnderCursor()
    {
        var hit = Physics.Raycast(
            Managers.MainCamera.ScreenPointToRay(Input.mousePosition),
            out var hitInfo,
            100f,
            Layers.CharacterMask);
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