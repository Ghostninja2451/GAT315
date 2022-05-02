using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extenstion
{
    public static Vector2 Wrap(this Vector2 v, Vector2 min, Vector2 max)
    {
        Vector2 result;

        result.x = v.x = (v.x > max.x) ? min.x : (v.x < min.x) ? max.x : v.x;
        result.y = v.y = (v.y > max.y) ? min.y : (v.y < min.y) ? max.y : v.y;

        return result;
    }


}
