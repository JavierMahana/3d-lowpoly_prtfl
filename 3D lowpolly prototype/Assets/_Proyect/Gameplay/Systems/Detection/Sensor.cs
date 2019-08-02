using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class Sensor : MonoBehaviour
{
    public SensorController.SensorType SensorType;
    
    private SphereCollider sensorCollider;
    private SensorController sensorController;
    private Unit unit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        Entity entity = other.GetComponentInParent<Entity>();
        if (entity != null && this.unit != entity) 
        {
            if (FiltrateEntity(entity))
            {
                sensorController.EntityEnterRange(entity, SensorType);

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger)
            return;

        Entity entity = other.GetComponentInParent<Entity>();
        if (entity != null && this.unit != entity)
        {
            if (FiltrateEntity(entity))
            {
                sensorController.EntityExit(entity, SensorType);
            }
            
        }
    }


    private void Start()
    {
        unit = transform.root.GetComponent<Unit>();
        Debug.Assert(unit != null);
        SetSensorController();
        SetupSensorCollider();
    }
    private bool FiltrateEntity(Entity entity)
    {
        if (FiltrateByTeam(entity))
        {
            if (unit.InteractOnlyWithUnits)
            {
                if (entity is Unit)
                {
                    return true;
                }
            }
            else
                return true;
        }
        return false;
    }
    private bool FiltrateByTeam(Entity entity)
    {
        switch (unit.InteractablesTeams)
        {
            case Unit.TeamsToInteract.SELF | Unit.TeamsToInteract.OTHER:
                return true;


            case Unit.TeamsToInteract.SELF:
                if (unit.Team == entity.Team)
                    return true;
                break;


            case Unit.TeamsToInteract.OTHER:
                if (unit.Team != entity.Team)
                    return true;
                break;
        }
        return false;
    }
    private void SetSensorController()
    {
        sensorController = transform.GetComponentInParent<SensorController>();
        if (sensorController == null)
        {
            Debug.LogError("There must be a sensor controller in his parent");
        }
    }
    private void SetupSensorCollider()
    {
        if (sensorCollider == null)
        {
            sensorCollider = GetComponent<SphereCollider>();
        }
        sensorCollider.isTrigger = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (sensorCollider != null)
        {
            Gizmos.DrawWireSphere(this.transform.position, sensorCollider.radius);
        }

        
    }
}
