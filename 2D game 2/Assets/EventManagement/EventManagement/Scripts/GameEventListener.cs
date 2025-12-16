using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CustomUnityEvent: UnityEvent <Component, object> {}
public class GameEventListener : MonoBehaviour
{
    public GameEvent m_gameEvent;
    public CustomUnityEvent m_gameResponse;

    private void OnEnable()
    {
        m_gameEvent.RegisterListener(this);
       
    }

    private void OnDisable()
    {
        m_gameEvent.UnRegisterListener(this);
    }

    public void OnEventsRaised(Component _sender, object _data)
    {

        m_gameResponse.Invoke(_sender, _data);
    }


}
