using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWarpData : MonoBehaviour
{
    public static Vector3? nextSpawnPosition = null;
    public static bool isWarping = false;

    public static void Save(Vector3 pos)
    {
        nextSpawnPosition = pos;
        isWarping = true;
    }

    public static Vector3 GetPositionOrDefault(Vector3 defaultPos)
    {
        return nextSpawnPosition.HasValue ? nextSpawnPosition.Value : defaultPos;
    }

    public static void Clear()
    {
        nextSpawnPosition = null;
        isWarping = false;
    }
}
