using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Lean.Pool;

public class ObjectFactory : MonoBehaviour
{
    [Required]
    public EntityIDToSprite SpriteDictionary;
    [Required]
    public EntityIDToPreview PlacementPreviewDictionary;
    [Required]
    public EntityIDToEntity FrindlyEntitiesDictionary;
    //public EntityIDToEntity EnemyEntitiesDictionary;


    //la entidades deberian tener diccionarios varios.
    //uno para cada equipo.
    public Entity InstantiateEntity(EntityID id, Vector3 worldPos, Transform parent,   bool friendly = true)
    {
        if (friendly){}
        else{}

        Entity newEntity = FrindlyEntitiesDictionary[id];
        if (newEntity == null)
        {
            Debug.LogError($"Entity dictionary doesn't have an entry for {id.ToString()}");
        }
        newEntity = LeanPool.Spawn(newEntity, worldPos, Quaternion.identity, parent);

        
        if (newEntity is Unit)
        {
            newEntity.transform.position += new Vector3(Random.Range(-unitOffSetRange, unitOffSetRange), 0, Random.Range(-unitOffSetRange, unitOffSetRange));
        }
        return newEntity;
    }
    public Entity InstantiateEntity(EntityID id, Vector3 worldPos, bool friendly = true)
    {
        if (friendly) { }
        else { }

        Entity newEntity = FrindlyEntitiesDictionary[id];
        if (newEntity == null)
        {
            Debug.LogError($"Entity dictionary doesn't have an entry for {id.ToString()}");
        }
        newEntity = LeanPool.Spawn(newEntity, worldPos, Quaternion.identity);

        if (newEntity is Unit)
        {
            newEntity.transform.position += new Vector3(Random.Range(-unitOffSetRange, unitOffSetRange), 0, Random.Range(-unitOffSetRange, unitOffSetRange));
        }
        return newEntity;
    }

    private const float unitOffSetRange = 0.01f;
}
