using System.Collections.Generic;
using UnityEngine;

public static class ClickLog
{
    private static float mouseButtonUpTime = 0f;

    public static bool GetMouseButtonUp()
    {
        if (!Input.GetMouseButtonUp(0))
            return false;

        if (Time.time == mouseButtonUpTime)
            return false;

        mouseButtonUpTime = Time.time;
        return true;
    }
}