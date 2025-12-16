using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "GameEvent")] 
public class GameEvent : ScriptableObject
{
    public List <GameEventListener> Listeners = new List <GameEventListener>();

    public void Raise(Component _sender, object _data)
    {
        for (int i = 0; i < Listeners.Count; i++)
        {
            Listeners[i].OnEventsRaised(_sender, _data);
        }
    }

    public void RegisterListener(GameEventListener _listener)
    {
        if(!Listeners.Contains(_listener))
        {
            Listeners.Add(_listener);
        }

    }

    public void UnRegisterListener(GameEventListener _listener)
    {
        if(Listeners.Contains(_listener))
        {
            Listeners.Remove(_listener);
        }
    }


}
