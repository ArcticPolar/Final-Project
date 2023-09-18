using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventEffect
{
    // VARIABLES

    [SerializeField] private string eventName;
    [SerializeField] private int amount;

    private Event eventA; // using eventA because event is a keyword



    // GET FUNCTIONS

    /// <summary>
    /// Get the name of the event effect
    /// </summary>
    /// <returns>The name of the event</returns>
    public string GetEventName()
    {
        return this.eventName;
    }

    /// <summary>
    /// Get the amount of the event effect
    /// </summary>
    /// <returns>The amount of the event effect</returns>
    public int GetEventAmount()
    {
        return this.amount;
    }



    /// <summary>
    /// Links the event name to the corresponding event in the given events list
    /// </summary>
    /// <param name="events">The list of events</param>
    public void LinkEvent(Events events)
    {
        this.eventA = events.GetEventByName(this.eventName);
    }

    /// <summary>
    /// Apply the effect.
    /// This adds the chance to the event
    /// </summary>
    public void ApplyEffect()
    {
        this.eventA.AddChance(this.amount);
    }
}
