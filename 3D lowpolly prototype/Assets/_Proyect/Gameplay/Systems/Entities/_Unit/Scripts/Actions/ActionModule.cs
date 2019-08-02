using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ActionModule : ScriptableObject
{
    public float DelayBetweenActions;
    public float ActionRange;

    public abstract void Execute(Entity entity);
    
}
