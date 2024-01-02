using System.Collections.Generic;
using UnityEngine;

public static class ClickLog
{
    private static float lmbUpTime = 0f;
    private static float rmbUpTime = 0f;

    public static bool GetLmbUp()
    {
        if (!Input.GetMouseButtonUp(0))
            return false;

        if (Time.time == lmbUpTime)
            return false;

        lmbUpTime = Time.time;
        return true;
    }

    public static bool GetRmbUp()
    {
        if (!Input.GetMouseButtonUp(1))
            return false;

        if (Time.time == rmbUpTime)
            return false;

        rmbUpTime = Time.time;
        return true;
    }
}