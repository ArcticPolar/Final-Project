using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemDaily : Item
{
    // VARIABLES

    [SerializeField] private ResourceEffect effect;



    // GET FUNCTIONS

    /// <summary>
    /// Get the resource effect of the item
    /// </summary>
    /// <returns>The resource effect of the item</returns>
    public ResourceEffect GetItemEffect()
    {
        return this.effect;
    }



    /// <summary>
    /// Apply the item effect
    /// </summary>
    public override void ApplyItemEffect()
    {
        // set the multiplier to be a default 1, 1
        this.effect.SetOpinionMultiplier(1.0f, 1.0f);
        this.effect.ApplyEffect();
    }
}
