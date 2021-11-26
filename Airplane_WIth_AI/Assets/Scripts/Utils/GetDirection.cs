using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GetDirection
{
    public static Vector3 DirectionTo(this Vector3 value, Vector3 to)
    {
        var vectorSubtract = value - to;
        var distance = vectorSubtract.magnitude;
        var direction = vectorSubtract / distance;
        return direction;
    }
    public static float OneDirectionTo(this float value, float to)
    {
        var direction = value - to;
        return Mathf.Clamp(direction,-1,1);
    }
}
