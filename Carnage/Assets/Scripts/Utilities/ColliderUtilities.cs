using System.Collections.Generic;
using UnityEngine;

public static class ColliderUtilities
{
    public static bool Encompasses(BoxCollider encompasser, BoxCollider encompassee)
    {
        foreach (Vector3 point in GetVertices(encompassee))
        {
            if (!encompasser.bounds.Contains(point))
                return false;
        }
        return true;
    }

    public static Vector3[] GetVertices(BoxCollider collider)
    {
        Vector3[] points = new Vector3[8];
        Transform trans = collider.transform;
        Vector3 min = collider.center - collider.size * 0.5f;
        Vector3 max = collider.center + collider.size * 0.5f;
        points[0] = trans.TransformPoint(new Vector3(min.x, min.y, min.z));
        points[1] = trans.TransformPoint(new Vector3(min.x, min.y, max.z));
        points[2] = trans.TransformPoint(new Vector3(min.x, max.y, min.z));
        points[3] = trans.TransformPoint(new Vector3(min.x, max.y, max.z));
        points[4] = trans.TransformPoint(new Vector3(max.x, min.y, min.z));
        points[5] = trans.TransformPoint(new Vector3(max.x, min.y, max.z));
        points[6] = trans.TransformPoint(new Vector3(max.x, max.y, min.z));
        points[7] = trans.TransformPoint(new Vector3(max.x, max.y, max.z));
        return points;
    }

    public static Vector3 GetCentroid(Vector3[] points)
    {
        Vector3 sum = Vector3.zero;
        foreach(Vector3 point in points)
        {
            sum += point;
        }
        return sum / points.Length;
    }

    public static Vector3 GetCentroid(ContactPoint[] points)
    {
        Vector3 sum = Vector3.zero;
        foreach (ContactPoint point in points)
        {
            sum += point.point;
        }
        return sum / points.Length;
    }

    public static Vector3 GetCentroid(List<ContactPoint> points)
    {
        Vector3 sum = Vector3.zero;
        foreach (ContactPoint point in points)
        {
            sum += point.point;
        }
        return sum / points.Count;
    }
}
