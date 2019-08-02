using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector2Extension
{
    public static float SqrDistance(this Vector2 a, Vector2 b)
    {
        float x = Mathf.Abs(a.x - b.x);
        float y = Mathf.Abs(a.y - b.y);

        x *= x;
        y *= y;

        return x + y;
    }
}
