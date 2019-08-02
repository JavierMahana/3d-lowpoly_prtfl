using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SensorController : MonoBehaviour
{
    public enum SensorType
    {
        DETECTION,
        ACTION,
    }
    private const float SENSOR_MINIMUM_OFFSET = 0.5F;

    public Entity ClosestEntityWithUnitPreference
    {
        get
        {
            bool haveUnit = false;
            if (EntitiesOnActionRange.Count != 0)
            {

                foreach (Entity entity in EntitiesOnActionRange)
                {
                    if (entity is Unit)
                    {
                        haveUnit = true;
                        break;
                    }
                }

                if (haveUnit)
                {
                    return ClosestUnitInSet(EntitiesOnActionRange);
                }
            }

            else if (EntitiesOnDetectionRange.Count != 0)
            {
                foreach (Entity entity in EntitiesOnDetectionRange)
                {
                    if (entity is Unit)
                    {
                        haveUnit = true;
                        break;
                    }
                }
                if (haveUnit)
                {
                    return ClosestUnitInSet(EntitiesOnDetectionRange);
                }
            }

            

            return ClosestEntity;
        }
    }
    public Entity ClosestEntity
    {
        get
        {
            if (EntitiesOnActionRange.Count != 0)
            {
                return ClosestEntityInHashSet(EntitiesOnActionRange);
            }
            else if (EntitiesOnDetectionRange.Count != 0)
            {
                return ClosestEntityInHashSet(EntitiesOnDetectionRange);
            }
            else
                return null;
        }
    }

    /// <summary>
    /// los elementos de una lista no pueden estar en la otra. Los elementos de deteccion SIEMPRE estarán más lejos que los de acción.
    /// </summary>
    private HashSet<Entity> EntitiesOnDetectionRange = new HashSet<Entity>();
    private HashSet<Entity> EntitiesOnActionRange = new HashSet<Entity>();
    private Animator animator;
    private Unit unit;


    private void OnEnable()
    {
        EnsureThatTheSensorsAreSetUpCorrectly();
        animator = GetComponentInParent<Animator>();
        unit = GetComponentInParent<Unit>();
    }

    public void OnEntityDisable(Entity other)
    {
        bool haveChanged = false;
        if (EntitiesOnDetectionRange.Contains(other))
        {
            EntitiesOnDetectionRange.Remove(other);
            haveChanged = true;
        }
        else if (EntitiesOnActionRange.Contains(other))
        {
            EntitiesOnActionRange.Remove(other);
            haveChanged = true;
        }

        if (haveChanged)
        {

            UpdateAnimatorBools();
            UpdateUnitTarget();
        }
    }
    public void EntityEnterRange(Entity other, SensorType type)
    {
        switch (type)
        {
            case SensorType.DETECTION:
                ListenOnDisableEvent(other);

                if (animator.GetBool("EntityOnDetectionRange") == false)
                    SetAnimatorBool("EntityOnDetectionRange", true);

                if (EntitiesOnActionRange.Contains(other))
                    EntitiesOnActionRange.Remove(other);

                EntitiesOnDetectionRange.Add(other);
                break;


            case SensorType.ACTION:
                if (animator.GetBool("EntityOnActionRange") == false)
                    SetAnimatorBool("EntityOnActionRange", true);

                if (EntitiesOnDetectionRange.Contains(other))
                    EntitiesOnDetectionRange.Remove(other);

                EntitiesOnActionRange.Add(other);
                break;


            default:
                break;
        }
        UpdateUnitTarget();
    }
    public void EntityExit(Entity other, SensorType type)
    {
        switch (type)
        {
            case SensorType.DETECTION:
                StopListeningOnDisableEvent(other);

                if (EntitiesOnDetectionRange.Contains(other) == false)
                    break;

                EntitiesOnDetectionRange.Remove(other);
                if (EntitiesOnDetectionRange.Count == 0)
                    SetAnimatorBool("EntityOnDetectionRange", false);

                break;


            case SensorType.ACTION:
                if (EntitiesOnActionRange.Contains(other) == false)
                    break;


                if (EntitiesOnDetectionRange.Contains(other) == false)
                    EntitiesOnDetectionRange.Add(other);



                EntitiesOnActionRange.Remove(other);
                if (EntitiesOnActionRange.Count == 0)
                    SetAnimatorBool("EntityOnActionRange", false);

                break;


            default:
                break;
        }

        UpdateUnitTarget();
    }

    private void ListenOnDisableEvent(Entity other)
    {
        other.OnEntityDisabled += OnEntityDisable;
    }
    private void StopListeningOnDisableEvent(Entity other)
    {
        other.OnEntityDisabled -= OnEntityDisable;
    }

    private void SetAnimatorBool(string parameterName, bool value)
    {
        if (animator != null)
        {
            animator.SetBool(parameterName, value);
        }
    }
    private void UpdateAnimatorBools()
    {
        if (EntitiesOnActionRange.Count == 0)
            SetAnimatorBool("EntityOnActionRange", false);
        else
            SetAnimatorBool("EntityOnActionRange", true);
        if (EntitiesOnDetectionRange.Count == 0)
            SetAnimatorBool("EntityOnDetectionRange", false);
        else
            SetAnimatorBool("EntityOnDetectionRange", true);
    }
    private void UpdateUnitTarget(bool unitPreference = true)
    {
        if (unitPreference)
            unit.Target = ClosestEntityWithUnitPreference;
        else
            unit.Target = ClosestEntity;
    }

    private Entity ClosestUnitInSet(HashSet<Entity> set)
    {
        HashSet<Entity> unitSet = UnitsInSet(set);
        return ClosestEntityInHashSet(unitSet);
    }
    private Entity ClosestEntityInHashSet(HashSet<Entity> set)
    {
        Entity closestEntity = null;
        float closestEntitySqrDistance = float.MaxValue;
        foreach (Entity entity in set)
        {
            if (closestEntity == null)
            {
                closestEntity = entity;
                closestEntitySqrDistance = this.transform.position.SqrDistanceXZ(entity.transform.position);
                continue;
            }
            float tempSqrDistance = this.transform.position.SqrDistanceXZ(entity.transform.position);
            if (closestEntitySqrDistance > tempSqrDistance)
            {
                closestEntity = entity;
                closestEntitySqrDistance = tempSqrDistance;
            }
        }
        return closestEntity;
    }
    private HashSet<Entity> UnitsInSet(HashSet<Entity> set)
    {
        HashSet<Entity> unitSet = new HashSet<Entity>();
        foreach (Entity entity in set)
        {
            if (entity is Unit)
            {
                unitSet.Add(entity);
            }
        }
        return unitSet;
    }

    private void EnsureThatTheSensorsAreSetUpCorrectly()
    {
        Sensor[] sensors = transform.GetComponentsInChildren<Sensor>();
        Debug.Assert(sensors.Length == 2,"there must be two sensors as childs, one detection and one action");

        Sensor detection = null;
        Sensor action = null;
        foreach (Sensor sensor in sensors)
        {
            switch (sensor.SensorType)
            {
                case SensorType.DETECTION:
                    detection = sensor;
                    break;
                case SensorType.ACTION:
                    action = sensor;
                    break;
            }
        }
        Debug.Assert(detection != null, "there must be two sensors as childs, one detection and one action");
        Debug.Assert(action != null, "there must be two sensors as childs, one detection and one action");


        SphereCollider detectionCollider = detection.GetComponent<SphereCollider>();
        SphereCollider actionCollider = action.GetComponent<SphereCollider>();

        if (actionCollider.radius < detectionCollider.radius)
        {
            return;
        }
        else
        {
            detectionCollider.radius = actionCollider.radius + SENSOR_MINIMUM_OFFSET;
        }

    }


}
