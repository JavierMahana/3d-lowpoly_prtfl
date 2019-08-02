using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Game Event/Generic/Entity")]
public class EntityGameEvent : ScriptableObject
{

    private List<EntityEventListener> eventListeners = new List<EntityEventListener>();

    public void RaiseEvents(Entity argument)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
        {
            eventListeners[i].OnEventRaised(argument);
        }
    }

    public void AddListener(EntityEventListener listener)
    {
        if (eventListeners.Contains(listener))
        {
            return;
        }
        eventListeners.Add(listener);
    }
    public void RemoveListener(EntityEventListener listener)
    {
        if (eventListeners.Contains(listener))
        {
            eventListeners.Remove(listener);
        }
    }

}
