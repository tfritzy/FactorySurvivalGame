using System.Collections.Generic;
using UnityEngine;

public static class Bezier
{
    public static List<Vector3> GetPoints(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int resolution = 10)
    {
        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i <= resolution; i++)
        {
            float t = i / (float)resolution;
            points.Add(CalculateCubicBezierPoint(t, p0, p1, p2, p3));
        }

        return points;
    }

    public static Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0; //first term

        p += 3 * uu * t * p1; //second term

        p += 3 * u * tt * p2; //third term

        p += ttt * p3; //fourth term

        return p;
    }
}