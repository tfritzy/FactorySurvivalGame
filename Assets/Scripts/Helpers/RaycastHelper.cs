using Core;
using UnityEngine;

public static class RaycastHelper
{
    public static Point3Int? GetHexUnderCursor()
    {
        var hit = Physics.Raycast(
            Camera.main.ScreenPointToRay(Input.mousePosition),
            out var hitInfo,
            100f,
            Layers.HexMask);
        if (hit)
        {
            return WorldConversions.UnityPositionToHex(hitInfo.point);
        }
        else
        {
            return null;
        }
    }
}