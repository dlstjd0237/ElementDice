using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public class TurnEventBus : MonoBehaviour
{
    private static readonly IDictionary<TurnEnumType, UnityEvent> Events =
        new Dictionary<TurnEnumType, UnityEvent>();

    public static void Subscribe(TurnEnumType type, UnityAction listener)
    {
        if (Events.TryGetValue(type, out UnityEvent thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            Events.Add(type, thisEvent);
        }
    }

    public static void UnSubscribe(TurnEnumType type, UnityAction listener)
    {
        if (Events.TryGetValue(type, out UnityEvent thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void AllEventUnSubscribe(TurnEnumType type)
    {
        Events[type].RemoveAllListeners();
    }

    public static void Publish(TurnEnumType raceType)
    {
        if (Events.TryGetValue(raceType, out UnityEvent thisEvent))
        {
            thisEvent?.Invoke();
        }
    }

    public static void GetTotalEvents()
    {
        if(Events == null) return;

        for (int i = 0; i < Events.Count; i++)
        {
            UnityEventBase unityEventBase = Events.ElementAt(i).Value;
            Debug.Log(string.Format("이벤트 : {0}, 갯수 : {1}", Events.ElementAt(i).Key, GetListenerNumber(unityEventBase)));
        }
    }

    public static int GetListenerNumber(UnityEventBase unityEvent)
    {
        var field = typeof(UnityEventBase).GetField("m_Calls",
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
        var invokeCallList = field.GetValue(unityEvent);
        var property = invokeCallList.GetType().GetProperty("Count");
        return (int)property.GetValue(invokeCallList);
    }
}
