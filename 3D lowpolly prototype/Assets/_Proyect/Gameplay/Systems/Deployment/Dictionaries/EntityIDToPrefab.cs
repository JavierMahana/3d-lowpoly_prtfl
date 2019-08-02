using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Deployment/Dictionary/EntityID to prefab")]
public class EntityIDToPrefab : SerializedScriptableObject
{
    public GameObject this[EntityID Id]
    {
        get
        {
            dictionary.TryGetValue(Id, out GameObject go);
            return go;
        }
    }
    [SerializeField]
    [Required]
    private Dictionary<EntityID, GameObject> dictionary;
}

