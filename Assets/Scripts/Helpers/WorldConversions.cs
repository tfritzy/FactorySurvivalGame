using Core;
using UnityEngine;

public static class WorldConversions
{
    private const float HORIZONTAL_DIST = Constants.HEX_RADIUS * 1.5f;
    private const float VERTICAL_DIST = Constants.HEX_APOTHEM * 2;

    public static Vector3 HexToUnityPosition(Point3Int hexPosition)
    {
        float xF = HORIZONTAL_DIST * hexPosition.x;
        float zF = VERTICAL_DIST * hexPosition.y + (hexPosition.x % 2 == 1 ? Constants.HEX_APOTHEM : 0);
        float yF = hexPosition.z * Constants.HEX_HEIGHT;
        return new Vector3(xF, yF, zF);
    }

    public static Point2Int UnityPositionToHex(Vector3 unityPosition)
    {
        // TODO: This is not correct, but it's close enough for now
        int x = Mathf.RoundToInt(unityPosition.x / HORIZONTAL_DIST);
        int y = Mathf.RoundToInt(unityPosition.z / VERTICAL_DIST);
        return new Point2Int(x, y);
    }
}