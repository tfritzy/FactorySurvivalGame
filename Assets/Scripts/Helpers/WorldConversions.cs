using System.Drawing;
using Core;
using UnityEngine;

public static class WorldConversions
{
    private const float HORIZONTAL_DIST = Constants.HEX_APOTHEM * 2;
    private const float VERTICAL_DIST = Constants.HEX_RADIUS * 1.5f;

    public static Vector3 HexToUnityPosition(Point3Int hexPosition)
    {
        float xF = HORIZONTAL_DIST * hexPosition.x - (hexPosition.y % 2 == 1 ? Constants.HEX_APOTHEM : 0);
        float zF = VERTICAL_DIST * hexPosition.y;
        float yF = hexPosition.z * Constants.HEX_HEIGHT;
        return new Vector3(xF, yF, zF);
    }

    public static Point3Int UnityPositionToHex(Vector3 unityPosition)
    {
        Point3Int res =
            GridHelpers.PixelToEvenRPlusHeight(new Point3Float(unityPosition.x, unityPosition.z, unityPosition.y));
        res.y = -res.y;
        return res;
    }
}