using System;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine;
using System.Linq;

[CreateAssetMenu(menuName = "Manager/Selection")]
public class SelectionManager : ScriptableObject
{
    public const int MAX_SELECTED = 10;
    public const int RAY_MAX_DISTANCE = 999;

    public List<Entity> Selected;

    public Team PlayerTeam;

    public Action SelectionChanged = delegate { };



    private void OnSelctedEntityDeath(Entity entity)
    {

        if (Selected.Remove(entity))
        {
            SelectionChanged();
        }
    }


    public void PointSelection(Camera camera, Vector3 screenPoint)
    {
        Reset();

        Ray ray = camera.ScreenPointToRay(screenPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, RAY_MAX_DISTANCE, 1 << 13, QueryTriggerInteraction.Ignore))//entity layer
        {
            Entity entity = raycastHit.transform.GetComponentInParent<Entity>();
            if (entity != null)
            {
                Select(entity);
            }
        }
        SelectionChanged();
    }
    public void AreaSelection(Camera camera, Vector3 aSceenPoint, Vector3 bScreenPoint, PlayerUnitManager playerUnitManager)
    {
        Reset();

        Bounds b = ViewportUtility.GetViewportBoundsFromScreenPoints(camera, aSceenPoint, bScreenPoint);
        foreach (Entity entity in playerUnitManager.PlayerUnits)
        {
            if (b.Contains(CameraUtility.Instance.MainCamera.WorldToViewportPoint(entity.transform.position)))
            {
                if (entity is Unit)
                {
                    if (entity.Team == PlayerTeam)
                    {
                        Select(entity);
                    }
                }
            }
        }
        SelectionChanged();
    }
    public void Select(Entity entity)
    {
        if (Selected.Count >= MAX_SELECTED)
        {
            return;
        }
        Selected.Add(entity);

        entity.OnEntityDisabled += OnSelctedEntityDeath;

        if (entity.OnSelect != null)
        {
            entity.OnSelect();
        }
    }

    public void Reset(bool notityASelectionChange = false)
    {
        foreach (Entity entity in Selected)
        {
            if (entity == null)
                continue;

            entity.OnEntityDisabled -= OnSelctedEntityDeath;

            if (entity.OnDeselect != null)
            {
                entity.OnDeselect();
            }
            
        }
        Selected.Clear();


        if (notityASelectionChange)
        {
            SelectionChanged();
        }
        
    }

}

    

    
