using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RayUtility 
{
    public static bool EntityInScreenPoint(Vector2 point, int rayLength = 100)
    {
        Ray ray = CameraUtility.Instance.MainCamera.ScreenPointToRay(point);
        if (Physics.Raycast(ray, out RaycastHit hit, rayLength, 1 << 13, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform.GetComponentInParent<Entity>() != null)
            {
                return true;
            }
        }
        return false;

    }
}
