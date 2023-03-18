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
}