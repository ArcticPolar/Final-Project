using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Choice
{
    // VARIABLES

    [SerializeField] private string desc;
    [SerializeField] private string result;
    [SerializeField] private string[] traitNames;
    [SerializeField] private ResourceEffect[] resourceEffects;
    [SerializeField] private EventEffect[] eventEffects;
    [SerializeField] private ChanceEffect[] chanceEffects;
    [SerializeField] private OpinionEffect opinionEffect;

    private Trait[] traits;

    // the UI for this choice
    private Button button;
    private GameObject tooltip;
    private TMP_Text[] tooltipTraits;
    private TMP_Text[] tooltipResources;
    private TMP_Text[] tooltipEvents;
    private TMP_Text[] tooltipChances;
    private TMP_Text tooltipFail;



    // GET FUNCTIONS

    /// <summary>
    /// Get the choice description
    /// </summary>
    /// <returns>The choice description</returns>
    public string GetChoiceDesc()
    {
        return this.desc;
    }

    /// <summary>
    /// Get the choice result text
    /// </summary>
    /// <returns>The choice result text</returns>
    public string GetChoiceResult()
    {
        return this.result;
    }

    /// <summary>
    /// Get the choice traits
    /// </summary>
    /// <returns>The choice traits</returns>
    public Trait[] GetChoiceTraits()
    {
        return this.traits;
    }

    /// <summary>
    /// Get the choice resource effects
    /// </summary>
    /// <returns>The choice resource effects</returns>
    public ResourceEffect[] GetChoiceResourceEffects()
    {
        return this.resourceEffects;
    }

    /// <summary>
    /// Get the choice event effects
    /// </summary>
    /// <returns>The choice event effects</returns>
    public EventEffect[] GetChoiceEventEffects()
    {
        return this.eventEffects;
    }

    /// <summary>
    /// Get the choice chance effects
    /// </summary>
    /// <returns>The choice chance effects</returns>
    public ChanceEffect[] GetChoiceChanceEffects()
    {
        return this.chanceEffects;
    }

    /// <summary>
    /// Get the choice opinion effect
    /// </summary>
    /// <returns>The choice opinion effect</returns>
    public OpinionEffect GetOpinionEffect()
    {
        return this.opinionEffect;
    }



    /// <summary>
    /// Links the trait names to the corresponding traits in the given traits list
    /// </summary>
    /// <param name="traits">The list of traits</param>
    public void LinkTraits(Traits traits)
    {
        this.traits = new Trait[traitNames.Length];

        for (var i = 0; i < this.traits.Length; i++)
        {
            this.traits[i] = traits.GetTraitByName(this.traitNames[i]);
            if (this.traits[i] == null)
            {
                Debug.Log("<color=red>Error: </color> Trait " + this.traitNames[i] + " not found in " + this.desc + " ! Fix: check if the trait name typed in is correct");
            }
        }
    }

    /// <summary>
    /// Call the functions in the resource effects, event effects and chance effects to link the resources and events
    /// </summary>
    /// <param name="resources">The list of resources</param>
    /// <param name="events">The list of events</param>
    public void LinkEffectsResourcesEvents(Resources resources, Events events)
    {
        for(var i = 0; i < this.resourceEffects.Length; i++)
        {
            this.resourceEffects[i].LinkResource(resources);
        }

        for(var i = 0; i < this.eventEffects.Length; i++)
        {
            this.eventEffects[i].LinkEvent(events);
        }

        for (var i = 0; i < this.chanceEffects.Length; i++)
        {
            this.chanceEffects[i].LinkResource(resources);
        }
    }

    /// <summary>
    /// Call the function in resource effects to apply a multiplier to the resource affect
    /// </summary>
    /// <param name="costMultiplier">The multipler to apply to the resource affect (if cost)</param>
    /// <param name="gainMultiplier">The multipler to apply to the resource affect (if gain)</param>
    public void SetOpinionAffect(float costMultiplier, float gainMultiplier)
    {
        for(var i = 0; i < this.resourceEffects.Length; i++)
        {
            this.resourceEffects[i].SetOpinionMultiplier(costMultiplier, gainMultiplier);
        }
    }



    // UI FUNCTIONS

    /// <summary>
    /// Setup the UI of the button
    /// </summary>
    /// <param name="prefab">The button to instantiate and write to</param>
    /// <param name="parent">The parent object to set as the parent</param>
    /// <param name="position">The position of the button</param>
    /// <returns>The instantiated button for the choice</returns>
    public Button SetupButtonUI(Button prefab, GameObject parent, Vector2 position)
    {
        this.button = GameObject.Instantiate(prefab);
        this.button.transform.SetParent(parent.transform, false);
        this.button.transform.localPosition = position;
        this.button.GetComponentInChildren<TMP_Text>().text = this.desc;

        // compare the amounts - if the player lacks the required amount the button is not interactable
        for (var i = 0; i < this.resourceEffects.Length; i++)
        {
            bool comp = this.resourceEffects[i].CompareAmounts();
            if (comp) { this.button.interactable = true; }
            else { this.button.interactable = false; }
        }

        return this.button;
    }

    /// <summary>
    /// Enables and disables the choice button
    /// </summary>
    public void EnableDisableButton()
    {
        if (this.button.isActiveAndEnabled)
        {
            this.button.enabled = false;
            this.button.interactable = false;
        }
        else
        {
            this.button.interactable = true;
            this.button.enabled = true;
        }
    }

    /// <summary>
    /// Destroy the UI of the choice button
    /// </summary>
    public void DestroyButtonUI()
    {
        GameObject.Destroy(this.button.gameObject);
    }

    /// <summary>
    /// Setup the UI of the choice tooltip
    /// </summary>
    /// <param name="createdTooltip">The tooltip created for this choice</param>
    /// <param name="prefab">The text box to instantiate and write to</param>
    /// <param name="character">The current character. Allows the traits to be compared and colours changed</param>
    public void SetupTooltip(GameObject createdTooltip, TMP_Text prefab, Character character)
    {
        this.tooltip = createdTooltip;

        // get the positions of empty objects, which dictate where the instantiated text boxes should be
        Vector2 ttTraits = createdTooltip.transform.GetChild(0).localPosition;
        Vector2 ttResources = createdTooltip.transform.GetChild(1).localPosition;
        Vector2 ttChances = createdTooltip.transform.GetChild(2).localPosition;
        Vector2 ttEvents = createdTooltip.transform.GetChild(3).localPosition;
        Vector2 ttGeneric = createdTooltip.transform.GetChild(4).localPosition;

        // attach it to script that runs the tooltip check
        Tooltip tooltip = this.button.GetComponent<Tooltip>();
        tooltip.SetTooltip(this.tooltip);

        // create the tooltips
        TooltipFail(prefab, ttGeneric);
        TooltipTraits(prefab, ttTraits, character);
        TooltipEffects(prefab, ttResources, ttChances, ttEvents);

        // hide the fail tooltip if not needed
        if (this.button.IsInteractable())
        {
            this.tooltipFail.enabled = false;
        }
        else
        {
            this.tooltipFail.enabled = true;
            // hide the other tooltips
            for (var i = 0; i < this.tooltipTraits.Length; i++) { this.tooltipTraits[i].enabled = false; }
            for (var i = 0; i < this.tooltipResources.Length; i++) { this.tooltipResources[i].enabled = false; }
            for (var i = 0; i < this.tooltipChances.Length; i++) { this.tooltipChances[i].enabled = false; }
            for (var i = 0; i < this.tooltipEvents.Length; i++) { this.tooltipEvents[i].enabled = false; }
        }
    }

    /// <summary>
    /// Add a fail tooltip if the player doesn't have enough resources
    /// </summary>
    /// <param name="prefab">The text box to instantiate and write to</param>
    /// <param name="position">The base position of the text box</param>
    public void TooltipFail(TMP_Text prefab, Vector2 position)
    {
        this.tooltipFail = GameObject.Instantiate(prefab);
        this.tooltipFail.transform.SetParent(this.tooltip.transform);
        this.tooltipFail.transform.localPosition = position;
        this.tooltipFail.text = "You lack the resources to take this choice!";
    }

    /// <summary>
    /// Add the traits to the choice tooltip
    /// </summary>
    /// <param name="prefab">The text box to instantiate and write to</param>
    /// <param name="position">The base position of the text box</param>
    /// <param name="character">The current character. Allows the traits to be compared and colours changed</param>
    public void TooltipTraits(TMP_Text prefab, Vector2 position, Character character)
    {
        this.tooltipTraits = new TMP_Text[this.traits.Length];

        for (var i = 0; i < this.traits.Length; i++)
        {
            this.tooltipTraits[i] = GameObject.Instantiate(prefab);
            this.tooltipTraits[i].transform.SetParent(this.tooltip.transform);
            this.tooltipTraits[i].transform.localPosition = new Vector2(position.x, position.y - (i * 20));

            for (var j = 0; j < character.GetCharacterTraits().Length; j++)
            {
                // matching traits
                if (this.traits[i].GetTraitName() == character.GetCharacterTraits()[j].GetTraitName())
                {
                    this.tooltipTraits[i].color = Color.green;
                }
                // opposite traits
                if (this.traits[i].GetOppositeTrait().GetTraitName() == character.GetCharacterTraits()[j].GetTraitName())
                {
                    this.tooltipTraits[i].color = Color.red;
                }
            }

            this.tooltipTraits[i].text = this.traits[i].GetTraitName();
            this.tooltipTraits[i].fontSize = 16;
        }
    }

    /// <summary>
    /// Add the resource and event effects to the choice tooltip
    /// </summary>
    /// <param name="prefab">The text box to instantiate and write to</param>
    /// <param name="positionResources">The base position of the resource text boxes</param>
    /// <param name="positionChances">The base position of the chance text boxes</param>
    /// <param name="positionEvents">The base position of the event text boxes</param>
    public void TooltipEffects(TMP_Text prefab, Vector2 positionResources, Vector2 positionChances, Vector2 positionEvents)
    {
        this.tooltipResources = new TMP_Text[this.resourceEffects.Length];

        for (var i = 0; i < this.resourceEffects.Length; i++)
        {
            this.tooltipResources[i] = GameObject.Instantiate(prefab);
            this.tooltipResources[i].transform.SetParent(this.tooltip.transform);
            this.tooltipResources[i].transform.localPosition = new Vector2(positionResources.x, positionResources.y - (i * 20));
            this.tooltipResources[i].text = this.resourceEffects[i].GetEffectResourceName() + ": " + this.resourceEffects[i].GetMultipliedAmount();
            this.tooltipResources[i].fontSize = 16;
        }

        this.tooltipEvents = new TMP_Text[this.eventEffects.Length];

        for (var i = 0; i < this.eventEffects.Length; i++)
        {
            this.tooltipEvents[i] = GameObject.Instantiate(prefab);
            this.tooltipEvents[i].transform.SetParent(this.tooltip.transform);
            this.tooltipEvents[i].transform.localPosition = new Vector2(positionChances.x, positionChances.y - (i * 20));
            this.tooltipEvents[i].text = this.eventEffects[i].GetEventName() + ": " + this.eventEffects[i].GetEventAmount();
            this.tooltipEvents[i].fontSize = 16;
        }

        this.tooltipChances = new TMP_Text[this.chanceEffects.Length];

        for (var i = 0; i < this.tooltipChances.Length; i++)
        {
            this.tooltipChances[i] = GameObject.Instantiate(prefab);
            this.tooltipChances[i].transform.SetParent(this.tooltip.transform);
            this.tooltipChances[i].transform.localPosition = new Vector2(positionEvents.x, positionEvents.y - (i * 20));
            this.tooltipChances[i].text = this.chanceEffects[i].GetEffectChance() + "% - " + this.chanceEffects[i].GetEffectSuccessResult();
            this.tooltipChances[i].text += "<br>" + (100 - this.chanceEffects[i].GetEffectChance()) + "% - " + this.chanceEffects[i].GetEffectFailResult();
            this.tooltipChances[i].fontSize = 16; 
        }
    }

    /// <summary>
    /// Destroy the UI of the tooltip, includes the traits, effects, events and fail
    /// </summary>
    public void DestroyTooltip()
    {
        for (var i = 0; i < this.tooltipTraits.Length; i++)
        {
            GameObject.Destroy(this.tooltipTraits[i].gameObject);
        }
        for (var i = 0; i < this.tooltipResources.Length; i++)
        {
            GameObject.Destroy(this.tooltipResources[i].gameObject);
        }
        for (var i = 0; i < this.tooltipEvents.Length; i++)
        {
            GameObject.Destroy(this.tooltipEvents[i].gameObject);
        }
        for (var i = 0; i < this.tooltipChances.Length; i++)
        {
            GameObject.Destroy(this.tooltipChances[i].gameObject);
        }
        GameObject.Destroy(this.tooltipFail.gameObject);
        GameObject.Destroy(this.tooltip);
    }
}
