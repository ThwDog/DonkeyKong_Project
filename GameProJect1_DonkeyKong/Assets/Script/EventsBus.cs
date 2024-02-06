using System.Collections.Generic;
using UnityEngine.Events;

public class EventsBus
{
    private static readonly IDictionary<GameManager._state,UnityEvent>
        Events = new Dictionary<GameManager._state,UnityEvent>();

    public static void Subscribe(GameManager._state state ,UnityAction listener)
    {
        UnityEvent thisEvent;
        if(Events.TryGetValue(state,out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            Events.Add(state,thisEvent);
        }
    }

    public static void UnSubscribe(GameManager._state state ,UnityAction listener)
    {
        UnityEvent thisEvent;
        if(Events.TryGetValue(state,out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void publish(GameManager._state state)
    {
        UnityEvent thisEvent;

        if(Events.TryGetValue(state,out thisEvent))
        {
            thisEvent.Invoke();
        }
    }
}
