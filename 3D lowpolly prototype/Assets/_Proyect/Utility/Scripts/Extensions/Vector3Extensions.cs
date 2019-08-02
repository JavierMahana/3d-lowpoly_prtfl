using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extensions 
{
    public static float DistanceXZ(this Vector3 position, Vector3 other)
    {
        Vector2 pos2D = new Vector2(position.x, position.z);
        Vector2 other2D = new Vector2(other.x, other.z);

        return Vector2.Distance(pos2D, other2D);
    }

    public static float SqrDistanceXZ(this Vector3 position, Vector3 other)
    {
        Vector2 pos2D = new Vector2(position.x, position.z);
        Vector2 other2D = new Vector2(other.x, other.z);

        return pos2D.SqrDistance(other2D);
    }
    public static float SqrDistanceXZ(this Vector3 position, Vector2 otherXZ)
    {
        Vector2 pos2D = new Vector2(position.x, position.z);

        return pos2D.SqrDistance(otherXZ);
    }
}
