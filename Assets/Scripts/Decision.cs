using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Decision
{
    // VARIABLES

    [SerializeField] private string name;
    [SerializeField] private string desc;
    [SerializeField] private int weight; // this is the chance of it being chosen.
    [SerializeField] private Choice[] choices;



    // GET FUNCTIONS

    /// <summary>
    /// Get the decision name
    /// </summary>
    /// <returns>The decision name</returns>
    public string GetDecisionName()
    {
        return this.name;
    }

    /// <summary>
    /// Get the decision description
    /// </summary>
    /// <returns>The decision description</returns>
    public string GetDecisonDesc()
    {
        return this.desc;
    }

    /// <summary>
    /// Get the decision weight
    /// </summary>
    /// <returns>The decision weight</returns>
    public int GetDecisionWeight()
    {
        return this.weight;
    }

    /// <summary>
    /// Get the decision choices
    /// </summary>
    /// <returns>The decision choices</returns>
    public Choice[] GetDecisionChoices()
    {
        return this.choices;
    }



    /// <summary>
    /// Call the function in the choices to link the traits
    /// </summary>
    /// <param name="traits">The list of traits</param>
    public void LinkChoiceTraits(Traits traits)
    {
        for (var i = 0; i < this.choices.Length; i++)
        {
            this.choices[i].LinkTraits(traits);
        }
    }

    /// <summary>
    /// Call the functions in the choices to link the resources and events
    /// </summary>
    /// <param name="resources">The list of resources</param>
    /// <param name="events">The list of events</param>
    public void LinkEffectResourcesEvents(Resources resources, Events events)
    {
        for (var i = 0; i < this.choices.Length; i++)
        {
            this.choices[i].LinkEffectsResourcesEvents(resources, events);
        }
    }

    /// <summary>
    /// Add or remove the amount given.
    /// Keeps the weight greater than 1
    /// </summary>
    /// <param name="amount">The number to change the weight by</param>
    public void AddWeight(int amount)
    {
        this.weight += amount;
        if(this.weight < 1)
        {
            this.weight = 1;
        }
    }

    /// <summary>
    /// Call the function in choices to apply a multiplier to the resource affect
    /// </summary>
    /// <param name="costMultiplier">The multipler to apply to the resource affect (if cost)</param>
    /// <param name="gainMultiplier">The multipler to apply to the resource affect (if gain)</param>
    public void SetOpinionMultiplier(float costMultiplier, float gainMultiplier)
    {
        for(var i = 0; i < this.choices.Length; i++)
        {
            this.choices[i].SetOpinionAffect(costMultiplier, gainMultiplier);
        }
    }



    // UI FUNCTIONS

    /// <summary>
    /// Calls the function in choices to setup the UI of the choice button
    /// </summary>
    /// <param name="prefab">The text box to instantiate and write to</param>
    /// <param name="parent">The parent object to set as the parent</param>
    /// <param name="position">The position of the button</param>
    /// <returns>All the instantiated buttons for the choices</returns>
    public Button[] SetupButtonUI(Button prefab, GameObject parent, Vector2 position)
    {
        Button[] buttons = new Button[this.choices.Length];
        for(var i = 0; i < this.choices.Length; i++)
        {
            Button button = this.choices[i].SetupButtonUI(prefab, parent, position);
            buttons[i] = button;
        }
        return buttons;
    }

    /// <summary>
    /// Calls the function in choices to setup the UI of the choice tooltip
    /// </summary>
    /// <param name="tooltips">The tooltips created for these choices</param>
    /// <param name="prefab">The text box to instantiate and write to</param>
    /// <param name="character">The current character. Allows the traits to be compared and colours changed</param>
    public void SetupTooltip(GameObject[] tooltips, TMP_Text prefab, Character character)
    {
        for(var i = 0; i < this.choices.Length; i++)
        {
            this.choices[i].SetupTooltip(tooltips[i], prefab, character);
        }
    }

    /// <summary>
    /// Calls the functions in choices to destroy the UI and tooltips
    /// </summary>
    public void DestroyUI()
    {
        for(var i = 0; i < this.choices.Length; i++)
        {
            this.choices[i].DestroyButtonUI();
            this.choices[i].DestroyTooltip();
        }
    }

    /// <summary>
    /// Calls the functions to enable and disable the choice button
    /// </summary>
    public void EnableDisableButton()
    {
        for (var i = 0; i < this.choices.Length; i++)
        {
            this.choices[i].EnableDisableButton();
        }
    }
}
