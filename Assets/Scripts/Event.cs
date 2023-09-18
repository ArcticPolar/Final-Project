using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Event
{
    // The type of the event
    public enum EventType
    {
        chanceBased, // higher number, higher chance of the event happening
        goalBased // when goal is reached, event happens
    }

    // VARIABLES

    [SerializeField] private string name;
    [SerializeField] private string desc;
    [SerializeField] private int chance; // the chance of the event happening (chance based) or the current total towards the goal (goal based)
    [SerializeField] private int goal; // the goal in goal based target
    [SerializeField] private EventType eventType;
    [SerializeField] private ResourceEffect[] resourceEffects;



    // GET FUNCTIONS

    /// <summary>
    /// Get the event name
    /// </summary>
    /// <returns>The event name</returns>
    public string GetEventName()
    {
        return this.name;
    }

    /// <summary>
    /// Get the event description
    /// </summary>
    /// <returns>The event description</returns>
    public string GetEventDesc()
    {
        return this.desc;
    }

    /// <summary>
    /// Get the event chance. This is the chance of the event happening (chance based) or the current total towards the goal (goal based)
    /// </summary>
    /// <returns>The event chance</returns>
    public int GetEventChance()
    {
        return this.chance;
    }

    /// <summary>
    /// Get the event goal. This is the goal in goal based target
    /// </summary>
    /// <returns>The event goal</returns>
    public int GetEventGoal()
    {
        return this.goal;
    }

    /// <summary>
    /// Get the event type. Enum: chanceBased, goalBased
    /// </summary>
    /// <returns>The event type</returns>
    public EventType GetEventType()
    {
        return this.eventType;
    }

    /// <summary>
    /// Get the event resource effects
    /// </summary>
    /// <returns>The event resource effects</returns>
    public ResourceEffect[] GetEventResourceEffects()
    {
        return this.resourceEffects;
    }



    /// <summary>
    /// Links the success and fail resources to the corresponding resources in the given resources list
    /// </summary>
    /// <param name="resources">The list of resources</param>
    public void LinkEventResource(Resources resources)
    {
        for (var i = 0; i < this.resourceEffects.Length; i++)
        {
            this.resourceEffects[i].LinkResource(resources);
        }
    }

    /// <summary>
    /// Add or remove the amount given
    /// </summary>
    /// <param name="amount">The number to change the chance amount by</param>
    public void AddChance(int amount)
    {
        this.chance += amount;

        // keep it in the range 0 - 100 if chance based event
        if (this.eventType == EventType.chanceBased)
        {
            if (this.chance < 0) { this.chance = 0; }
            if (this.chance > 100) { this.chance = 100; }
        }
    }

    /// <summary>
    /// Apply the resource effects of the event
    /// </summary>
    public void ApplyEffect()
    {
        for(var i = 0; i < this.resourceEffects.Length; i++)
        {
            // set the multiplier to be a default 1, 1
            this.resourceEffects[i].SetOpinionMultiplier(1.0f, 1.0f);
            this.resourceEffects[i].ApplyEffect();
        }
    }

    /// <summary>
    /// Check if the event happens
    /// </summary>
    /// <returns>An event if it has happened, or null if it hasn't</returns>
    public Event CheckEvent()
    {
        // chance based event
        if(this.eventType == EventType.chanceBased)
        {
            int random_num = Random.Range(0, 100);

            if (random_num < this.chance)
            {
                // the event happens
                this.ApplyEffect();
                this.chance = 0;
                return this;
            }
            else
            {
                // the event doesn't happen! ...yet
                return null;
            }
        }
        // goal based event
        else
        {
            if(this.chance >= this.goal)
            {
                // the event happens
                this.ApplyEffect();
                this.chance = 0;
                return this;
            }
            else
            {
                // the event doesn't happen! ...yet
                return null;
            }
        }
    }
}
