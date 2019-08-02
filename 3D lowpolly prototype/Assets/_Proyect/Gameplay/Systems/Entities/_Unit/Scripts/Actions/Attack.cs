using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Entity/Unit/Action Module/Attack")]
public class Attack : ActionModule
{
    public int damage;

    public override void Execute(Entity entity)
    {
        entity.Health.ModifyHealth(-damage);
    }
}
