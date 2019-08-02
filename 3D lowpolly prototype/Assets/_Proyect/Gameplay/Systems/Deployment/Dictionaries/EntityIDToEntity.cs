using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Deployment/Dictionary/EntityID to entity")]
public class EntityIDToEntity : SerializedScriptableObject
{
    public Entity this[EntityID Id]
    {
        get
        {
            dictionary.TryGetValue(Id, out Entity entity);
            return entity;
        }
    }
    [SerializeField]
    [Required]
    public Dictionary<EntityID, Entity> dictionary;
}
