using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Deployment/Dictionary/EntityID to sprite")]
public class EntityIDToSprite : SerializedScriptableObject
{
    public Sprite this[EntityID Id]
    {
        get
        {
            dictionary.TryGetValue(Id, out Sprite sprite);
            return sprite;
        }
    }
    [SerializeField]
    [Required]
    private Dictionary<EntityID, Sprite> dictionary;
}
