using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemConstant : Item
{
    // VARIABLES

    [SerializeField] private int amount;

    

    // GET FUNCTIONS

    /// <summary>
    /// Get the amount added for the item effect
    /// </summary>
    /// <returns>The amount added</returns>
    public int GetItemAmount()
    {
        return this.amount;
    }



    /// <summary>
    /// Apply the item effect
    /// </summary>
    /// <param name="charSelected">The selected character</param>
    /// <param name="decSelected">The selected decision</param>
    /// <param name="isApply">Whether the effect is being applied or unapplied (so that it doesn't stack every single time)</param>
    public override void ApplyItemEffect(Character charSelected, Decision decSelected, bool isApply)
    {
        // apply the effect of the effect... this is hardcoded
        if (this.GetItemName() == "Excalibur")
        {
            // opinion plus 10
            if (isApply) { charSelected.AddOpinion(this.amount); }
            else { charSelected.AddOpinion(-this.amount); }

        }
        if(this.GetItemName() == "The One Ring")
        {
            // choice resource effect increased by 20
            Choice[] choices = decSelected.GetDecisionChoices();
            for(var i = 0; i < choices.Length; i++)
            {
                ResourceEffect[] effects = choices[i].GetChoiceResourceEffects();
                for (var j = 0; j < effects.Length; j++)
                {
                    if (isApply) { effects[j].AddResourceAmount(this.amount); }
                    else { effects[j].AddResourceAmount(-this.amount); }
                }
            }
        }
        else
        {
            Debug.Log("<color=red>Error: </color> Item " + this.GetItemName() + " doesn't have an effect! Fix: check if the item name typed in is correct");
        }
    }
}
