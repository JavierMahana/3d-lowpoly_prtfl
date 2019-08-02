using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColliderExtensions
{


    public static Collider ClosestColliderXZ(this Collider collider, Collider[] others)
    {
        Vector3 startPosition = collider.transform.position;

        Collider closestEnemy = others[0];
        if (closestEnemy == null)
        {
            return null;
        }

        float sqrMinDistance = startPosition.SqrDistanceXZ(closestEnemy.transform.position);

        for (int i = 1; i < others.Length; i++)
        {
            Collider check = others[i];
            float checkSqrDist = startPosition.SqrDistanceXZ(check.transform.position);
            if (checkSqrDist < sqrMinDistance)
            {
                closestEnemy = check;
                sqrMinDistance = checkSqrDist;
            }
        }
        return closestEnemy;
    }
    public static Collider ClosestColliderInAreaXZ(this Collider collider, float radious, LayerMask layerMask)
    {
        Vector3 startPosition = collider.transform.position;

        Vector2 pos2D = new Vector2(startPosition.x, startPosition.z);
        Collider[] enemies = Physics.OverlapSphere(startPosition, radious, layerMask);
        if (enemies.Length == 0)
        {
            return null;
        }
        Collider closestEnemy = enemies[0];
        Vector2 enemyPos = new Vector2(closestEnemy.transform.position.x, closestEnemy.transform.position.z);
        float minSqrDist = pos2D.SqrDistance(enemyPos);

        for (int i = 1; i < enemies.Length; i++)
        {
            enemyPos = new Vector2(enemies[i].transform.position.x, enemies[i].transform.position.z);
            float sqrDist = pos2D.SqrDistance(enemyPos);
            if (sqrDist < minSqrDist)
            {
                minSqrDist = sqrDist;
                closestEnemy = enemies[i];
            }
        }

        return closestEnemy;
    }
    public static bool ColliderInsideAnArea(this Collider collider, float radious, LayerMask colliderLayer)
    {
        Vector3 startPosition = collider.transform.position;
        Collider[] enemies = Physics.OverlapSphere(startPosition, radious, colliderLayer);
        if (enemies.Length == 0)
        {
            return false;

        }
        return true;
    }
    public static bool ColliderInsideAnArea(this Collider collider, float radious, LayerMask colliderLayer, out Collider[] results)
    {
        Vector3 startPosition = collider.transform.position;

        results = Physics.OverlapSphere(startPosition, radious, colliderLayer);
        if (results.Length == 0)
        {
            return false;

        }
        return true;
    }
    public static bool ColliderInsideAnAreaNonAlloc(this Collider collider, float radious, LayerMask colliderLayer, ref Collider[] results)
    {
        Vector3 startPosition = collider.transform.position;

        if (Physics.OverlapSphereNonAlloc(startPosition, radious, results, colliderLayer) > 0)
        {
            return true;
        }
        return false;
    }
}
