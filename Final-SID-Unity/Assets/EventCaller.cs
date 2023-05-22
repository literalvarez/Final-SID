using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventCaller : MonoBehaviour
{
    public UnityEvent awakeEvent;
    public UnityEvent delayedEvent;

    private void Start()
    {
        // Call the awakeEvent immediately on Awake
        awakeEvent.Invoke();

        // Call the delayedEvent after 0.1 seconds using Invoke
        Invoke("CallDelayedEvent", 0.1f);
    }

    private void CallDelayedEvent()
    {
        delayedEvent.Invoke();
    }
}