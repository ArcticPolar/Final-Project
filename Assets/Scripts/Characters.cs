using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Characters : MonoBehaviour
{
    // VARIABLES

    [SerializeField] private Character[] characters;



    // GET FUNCTIONS

    /// <summary>
    /// Get all the characters
    /// </summary>
    /// <returns>The list of characters</returns>
    public Character[] GetCharacters()
    {
        return this.characters;
    }



    /// <summary>
    /// Call the functions in the characters to link the traits, resources and events
    /// </summary>
    /// <param name="traits">The list of traits</param>
    /// <param name="resources">The list of resources</param>
    /// <param name="events">The list of events</param>
    public void LinkTraitsAndResources(Traits traits, Resources resources, Events events)
    {
        for (var i = 0; i < this.characters.Length; i++)
        {
            this.characters[i].LinkTraits(traits);
            this.characters[i].LinkChoiceTraits(traits);
            this.characters[i].LinkEffectResourcesEvents(resources, events);
        }
    }

    /// <summary>
    /// Select a character from the possible characters using weighted probability
    /// </summary>
    /// <returns>The character selected or null if no character is selected (for debug purposes)</returns>
    public Character SelectCharacter()
    {
        // step 1
        int weights_sum = 0;
        for (var i = 0; i < this.characters.Length; i++)
        {
            weights_sum += this.characters[i].GetCharacterWeight();
        }

        // step 2
        int random_num = Random.Range(0, weights_sum);

        // step 3
        for (var i = 0; i < this.characters.Length; i++)
        {
            if (random_num < this.characters[i].GetCharacterWeight())
            {
                this.IncreaseWeights(i);
                return this.characters[i];
            }
            random_num -= this.characters[i].GetCharacterWeight();
        }
        Debug.Log("<color=red>Error: </color> No character selected! Fix: check if character weights are correct");
        return null;
    }

    /// <summary>
    /// Increase the weights of the characters that weren't selected and decrease the weight of the character that was selected
    /// </summary>
    /// <param name="charSelectedIdx">The index of the selected character</param>
    public void IncreaseWeights(int charSelectedIdx)
    {
        for (var i = 0; i < this.characters.Length; i++)
        {
            if (i == charSelectedIdx)
            {
                this.characters[i].AddWeight(-5);
            }
            else
            {
                this.characters[i].AddWeight(5);
            }
        }
    }
}
