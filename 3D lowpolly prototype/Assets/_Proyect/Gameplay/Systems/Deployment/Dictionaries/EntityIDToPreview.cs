using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Deployment/Dictionary/EntityID to preview")]
public class EntityIDToPreview : SerializedScriptableObject
{

    public DeploymentPreview this[EntityID Id]
    {
        get
        {
            dictionary.TryGetValue(Id, out DeploymentPreview preview);
            return preview;
        }
    }
    [SerializeField]
    [Required]
    public Dictionary<EntityID, DeploymentPreview> dictionary;
}
