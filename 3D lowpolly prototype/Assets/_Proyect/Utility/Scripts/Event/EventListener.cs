using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class EventListener : MonoBehaviour
{

    public GameEvent Event;


    public UnityEvent Response;



    public void OnEventRaised()
    {
        Response.Invoke();
    }

    private void OnEnable()
    {
        Event.AddListener(this);
    }
    private void OnDisable()
    {
        Event.RemoveListener(this);
    }
}
//[System.Serializable]
//public class EventListener<T> : MonoBehaviour
//{


//    public GameEvent<T> Event;

//    public UnityEvent<T> Response;



//    public void OnEventRaised(T argument)
//    {
        
//        Response.Invoke(argument);
//    }

//    private void OnEnable()
//    {
//        Event.AddListener(this);
//    }
//    private void OnDisable()
//    {
//        Event.RemoveListener(this);
//    }
//}
