using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Game Event/Default")]
public class GameEvent : ScriptableObject
{
    private List<EventListener> eventListeners = new List<EventListener>();

    public void RaiseEvents()
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
        {
            eventListeners[i].OnEventRaised();
        }
    }

    public void AddListener(EventListener listener)
    {
        if (eventListeners.Contains(listener))
        {
            return;
        }
        eventListeners.Add(listener);
    }
    public void RemoveListener(EventListener listener)
    {
        if (eventListeners.Contains(listener))
        {
            eventListeners.Remove(listener);
        }
    }


}

