using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events : MonoBehaviour
{
    // VARIABLES

    [SerializeField] private Event[] events;



    // GET FUNCTIONS

    /// <summary>
    /// Get all the events
    /// </summary>
    /// <returns>The list of events</returns>
    public Event[] GetEvents()
    {
        return this.events;
    }

    /// <summary>
    /// Iterate through the events to find the given event name
    /// </summary>
    /// <param name="eventName">The name of the event being searched for</param>
    /// <returns>The requested event or null if the event isn't found (for debug purposes)</returns>
    public Event GetEventByName(string eventName)
    {
        for (var i = 0; i < this.events.Length; i++)
        {
            if (this.events[i].GetEventName() == eventName)
            {
                // using eventA because event is a keyword
                Event eventA = this.events[i];
                return eventA;
            }
        }
        Debug.Log("<color=red>Error: </color> Event " + eventName + " not found! Fix: check if the event name typed in is correct");
        return null;
    }



    /// <summary>
    /// Check if any of the events happen
    /// </summary>
    /// <returns>The events that have happened (null for events that haven't)</returns>
    public Event[] CheckEvents()
    {
        Event[] eventsHappened = new Event[this.events.Length];
        for(var i = 0; i < this.events.Length; i++)
        {
            Event eventHappened = this.events[i].CheckEvent();
            eventsHappened[i] = eventHappened;
        }
        return eventsHappened;
    }

    /// <summary>
    /// Call the functions in the events to link the resources and events
    /// </summary>
    /// <param name="resources">The list of resources</param>
    public void LinkEventResource(Resources resources)
    {
        for (var i = 0; i < this.events.Length; i++)
        {
            this.events[i].LinkEventResource(resources);
        }
    }
}
