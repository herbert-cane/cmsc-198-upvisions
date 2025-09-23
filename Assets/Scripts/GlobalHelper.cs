using UnityEngine;

public static class GlobalHelper
{
    public static string GenerateUniqueID(GameObject obj)
    {
        if (obj == null)
            return System.Guid.NewGuid().ToString();

        var pos = obj.transform.position;
        return $"{obj.scene.name}_{pos.x}_{pos.y}";
    }
}