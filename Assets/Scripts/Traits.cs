using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traits : MonoBehaviour
{
    // VARIABLES

    [SerializeField] private Trait[] traits;



    // GET FUNCTIONS

    /// <summary>
    /// Get all the traits
    /// </summary>
    /// <returns>The list of traits</returns>
    public Trait[] GetTraits()
    {
        return this.traits;
    }

    /// <summary>
    /// Iterate through the traits to find the given trait name
    /// </summary>
    /// <param name="traitName">The name of the trait being searched for</param>
    /// <returns>The requested trait or null if the trait isn't found (for debug purposes)</returns>
    public Trait GetTraitByName(string traitName)
    {
        for (var i = 0; i < this.traits.Length; i++)
        {
            if (this.traits[i].GetTraitName() == traitName)
            {
                Trait trait = this.traits[i];
                return trait;
            }
        }
        Debug.Log("<color=red>Error: </color> Trait " + traitName + " not found! Fix: check if the trait name typed in is correct");
        return null;
    }



    /// <summary>
    /// Iterate through the traits and assign their opposite trait.
    /// This has to be done after all traits have been initialised
    /// </summary>
    public void AddOppositeTraits()
    {
        for(var i = 0; i < this.traits.Length; i++)
        {
            this.traits[i].AddOppositeTrait(this.GetTraitByName(this.traits[i].GetGivenOppositeTraitName()));
        }
    }

}
