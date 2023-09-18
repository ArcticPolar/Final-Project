using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class Objective
{
    // The type of the objective
    public enum Type
    {
        resource,
        people,
        days
    }

    // VARIABLES

    [SerializeField] private string name;
    [SerializeField] private string desc;
    [SerializeField] private string resourceName;
    [SerializeField] private int amount;
    [SerializeField] private string rewardName;
    [SerializeField] private Type type;

    private Item reward;
    private bool completed;
    private Resource resource;

    private TMP_Text objectiveUI;



    // GET FUNCTIONS

    /// <summary>
    /// Get the objective name
    /// </summary>
    /// <returns>The objective name</returns>
    public string GetObjectiveName()
    {
        return this.name;
    }

    /// <summary>
    /// Get the objective description
    /// </summary>
    /// <returns>The objective description</returns>
    public string GetObjectiveDesc()
    {
        return this.desc;
    }

    /// <summary>
    /// Get the objective reward
    /// </summary>
    /// <returns>The objective reward</returns>
    public Item GetObjectiveReward()
    {
        return this.reward;
    }

    /// <summary>
    /// Get the objective type
    /// </summary>
    /// <returns>The objective type</returns>
    public Type GetObjectiveType()
    {
        return this.type;
    }



    /// <summary>
    /// Links the resource name to the corresponding resource in the given resources list
    /// </summary>
    /// <param name="resources">The list of resources</param>
    public void LinkResource(Resources resources)
    {
        if (this.type == Type.resource)
        {
            this.resource = resources.GetResourceByName(this.resourceName);
        }
    }

    /// <summary>
    /// Links the item reward to the corresponding item in the given items list.
    /// Also links this objective to that item
    /// </summary>
    /// <param name="items">The list of items</param>
    public void LinkReward(Items items)
    {
        this.reward = items.GetItemByName(this.rewardName);
        this.reward.SetUnlockObjective(this);
    }

    /// <summary>
    /// Check if the objective has been completed by comparing to the days, people or resource amount.
    /// The resource has already linked so isn't needed as a parameter
    /// </summary>
    /// <param name="days">The number of days played</param>
    /// <param name="people">The number of people dealt with</param>
    public void CheckObjective(int days, int people)
    {
        // If a days objective
        if(this.type == Type.days)
        {
            // have we reached the day number
            if(this.amount == days)
            {
                // we did it
                this.completed = true;
                this.ApplyEffect();
            }
        }

        // If a people objective
        if (this.type == Type.people)
        {
            // have we reached the day number
            if (this.amount == people)
            {
                // we did it
                this.completed = true;
                this.ApplyEffect();
            }
        }

        // If a resource objective
        if (this.type == Type.resource)
        {
            // have we reached the day number
            if (this.amount <= this.resource.GetResourceAmount())
            {
                // we did it
                this.completed = true;
                this.ApplyEffect();
            }
        }
    }

    /// <summary>
    /// Apply the effect of the objective.
    /// This currently gives the player the reward item
    /// </summary>
    public void ApplyEffect()
    {
        this.reward.SetPlayerHas(true);
    }



    // UI FUNCTIONS //

    /// <summary>
    /// Setup the UI of the objective
    /// </summary>
    /// <param name="prefab">The text box to instantiate and write to</param>
    /// <param name="parent">The parent object to set as the parent</param>
    /// <param name="position">The position of the text box</param>
    public void SetupUI(TMP_Text prefab, GameObject parent, Vector2 position)
    {
        this.objectiveUI = GameObject.Instantiate(prefab);
        this.objectiveUI.transform.SetParent(parent.transform, false);
        this.objectiveUI.transform.localPosition = position;
    }

    /// <summary>
    /// Update the UI of the objective
    /// </summary>
    public void UpdateUI()
    {
        this.objectiveUI.text = this.name + "<br>" + this.desc;
        if (this.completed == true) { this.objectiveUI.text += "<br>Completed!"; }
        else { this.objectiveUI.text += "<br>Reward: " + this.reward.GetItemName(); }
    }
}
