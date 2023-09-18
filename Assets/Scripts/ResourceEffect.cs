using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceEffect
{
    // VARIABLES

    [SerializeField] private string resourceName;
    [SerializeField] private int amount;
    private float multiplier;

    private Resource resource;



    // GET FUNCTIONS

    /// <summary>
    /// Get the name of the resource effect
    /// </summary>
    /// <returns>The name of the resource effect</returns>
    public string GetEffectResourceName()
    {
        return this.resourceName;
    }

    /// <summary>
    /// Get the amount of the resource effect
    /// </summary>
    /// <returns>The amount of the resource effect</returns>
    public int GetEffectResourceAmount()
    {
        return this.amount;
    }

    /// <summary>
    /// Get the multiplier of the resource effect
    /// </summary>
    /// <returns>The multiplier of the resource effect</returns>
    public float GetEffectResourceMultiplier()
    {
        return this.multiplier;
    }

    /// <summary>
    /// Get the multipied amount (amount times the multiplier)
    /// </summary>
    /// <returns>The multiplied amount</returns>
    public int GetMultipliedAmount()
    {
        float temp = this.amount;
        temp *= this.multiplier;
        return Mathf.RoundToInt(temp);
    }



    // SET FUNCTIONS

    /// <summary>
    /// Apply a multiplier to the resource affect amount
    /// </summary>
    /// <param name="costMultiplier">The multipler to apply to the resource affect (if cost)</param>
    /// <param name="gainMultiplier">The multipler to apply to the resource affect (if gain)</param>
    public void SetOpinionMultiplier(float costMultiplier, float gainMultiplier)
    {
        // if amount is negative, it is a cost
        if(this.amount < 0)
        {
            this.multiplier = costMultiplier;
        }
        // if amount if positive, it is a gain
        if(this.amount > 0)
        {
            this.multiplier = gainMultiplier;
        }
    }



    /// <summary>
    /// Links the resource name to the corresponding resource in the given resources list
    /// </summary>
    /// <param name="resources">The list of resources</param>
    public void LinkResource(Resources resources)
    {
        this.resource = resources.GetResourceByName(this.resourceName);
    }

    /// <summary>
    /// Apply the effect.
    /// This adds the amount to the resource
    /// </summary>
    public void ApplyEffect()
    {
        this.resource.AddResource(this.GetMultipliedAmount());
    }

    /// <summary>
    /// Add the given amount to the current resource amount
    /// </summary>
    /// <param name="amount">The amount added</param>
    public void AddResourceAmount(int amount)
    {
        this.amount += amount;
    }

    /// <summary>
    /// Compare the amount of the resource needed to the amount of the resource you have.
    /// </summary>
    /// <returns>True if the difference is greater than or equal to 0, or false if the difference is less than 0</returns>
    public bool CompareAmounts()
    {
        // if the amount is greater than or equal to 0, it doesn't matter - you don't have to pay any resources!
        if (this.amount < 0)
        {
            // compare what you need to the amount you have 
            if (Mathf.Abs(this.amount) > this.resource.GetResourceAmount())
            {
                // not enough of that resource
                return false;
            }
            else { return true; }
        }
        else { return true; }
    }
}
