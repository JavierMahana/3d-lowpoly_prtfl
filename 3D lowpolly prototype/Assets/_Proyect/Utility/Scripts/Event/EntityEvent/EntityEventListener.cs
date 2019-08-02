using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityEventListener : MonoBehaviour
{
    public EntityGameEvent Event;


    public EntityUnityEvent Response;



    public void OnEventRaised(Entity argument)
    {
        Response.Invoke(argument);
    }

    private void OnEnable()
    {
        if (Event != null)
        {
            Event.AddListener(this);
        }
    }
    private void OnDisable()
    {
        if (Event != null)
        {
            Event.RemoveListener(this);
        }
    }
}
