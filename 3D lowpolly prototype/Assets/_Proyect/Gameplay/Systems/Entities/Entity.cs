using System.Collections;
using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Health))]
public abstract class Entity : SerializedMonoBehaviour
{
    [FoldoutGroup("Entity Attributes")]
    [Required]
    public Team Team;
    [FoldoutGroup("Entity Attributes")]
    [Required]
    public EntityID Id;
    [FoldoutGroup("Entity Attributes")]
    [HideInInspector]
    public Health Health;

    [HideInInspector]
    public Action OnSelect = delegate { };
    [HideInInspector]
    public Action OnDeselect = delegate { };
    [HideInInspector]
    public Action<Entity> OnEntityDisabled = delegate { };


    private SelectionVisual selectionVisual;


    protected virtual void Start()
    {
        selectionVisual = GetComponentInChildren<SelectionVisual>();
        Health = GetComponent<Health>();
        Health.HealthsEntity = this;
    }
    protected virtual void OnEnable()
    {

    }
    protected virtual void OnDisable()
    {
        if (! aplicationQuit)
        {
            OnEntityDisabled(this);
        }
    }
    protected virtual void OnApplicationQuit()
    {
        aplicationQuit = true;
    }
    private bool aplicationQuit = false;
}
