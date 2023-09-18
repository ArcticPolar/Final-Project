using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class Character
{
    // VARIABLES

    [SerializeField] private string name;
    [SerializeField] private int opinion;
    [SerializeField] private int weight; // how likely this character will be selected.
    [SerializeField] private string[] traitNames;

    // array of traits this character has!
    private Trait[] traits;

    // array of decisions this character has!
    public Decision[] decisions;

    //sprite assignment
    [SerializeField] private GameObject characterRoot;
    [SerializeField] private GameObject characterSpritePrefab;
    private GameObject characterSprite;



    // GET FUNCTIONS

    /// <summary>
    /// Get the character name
    /// </summary>
    /// <returns>The character name</returns>
    public string GetCharacterName()
    {
        return this.name;
    }

    /// <summary>
    /// Get the character opinion
    /// </summary>
    /// <returns>The character opinion</returns>
    public int GetCharacterOpinion()
    {
        return this.opinion;
    }

    /// <summary>
    /// Get the character weight
    /// </summary>
    /// <returns>The character weight</returns>
    public int GetCharacterWeight()
    {
        return this.weight;
    }

    /// <summary>
    /// Get the character traits
    /// </summary>
    /// <returns>The character traits</returns>
    public Trait[] GetCharacterTraits()
    {
        return this.traits;
    }



    /// <summary>
    /// Links the trait names to the corresponding traits in the given traits list
    /// </summary>
    /// <param name="traits">The list of traits</param>
    public void LinkTraits(Traits traits)
    {
        this.traits = new Trait[this.traitNames.Length];

        for(var i = 0; i < this.traitNames.Length; i++)
        {
            this.traits[i] = traits.GetTraitByName(this.traitNames[i]);
            if (this.traits[i] == null)
            {
                Debug.Log("<color=red>Error: </color> Trait " + this.traitNames[i] + " not found in " + this.name + " ! Fix: check if the trait name typed in is correct");
            }
        }
    }

    /// <summary>
    /// Call the function in the decisions to link the traits
    /// </summary>
    /// <param name="traits">The list of traits</param>
    public void LinkChoiceTraits(Traits traits)
    {
        for(var i = 0; i < this.decisions.Length; i++)
        {
            this.decisions[i].LinkChoiceTraits(traits);
        }
    }

    /// <summary>
    /// Call the functions in the decisions to link the resources and events
    /// </summary>
    /// <param name="resources">The list of resources</param>
    /// <param name="events">The list of events</param>
    public void LinkEffectResourcesEvents(Resources resources, Events events)
    {
        for (var i = 0; i < this.decisions.Length; i++)
        {
            this.decisions[i].LinkEffectResourcesEvents(resources, events);
        }
    }

    /// <summary>
    /// Select a decision from this characters possible decisions using weighted probability
    /// </summary>
    /// <returns>The decision selected or null if no decision is selected (for debug purposes)</returns>
    public Decision SelectDecision()
    {
        // pseudocode algorithm
        // stackoverflow.com/questions/1761626/weighted-random-numbers

        // step 1: calculate the sum of all the weights
        // sum of weights = 0
        // for every decision weight
        // add to sum of weights

        // step 2: pick a random number that is > 0 and < sum of weights

        // step 3: for each decision, subtract the weight from randon number
        // until we get the decision where the random number is less than the weight of that decision

        // step 1
        int weights_sum = 0;
        for(var i = 0; i < this.decisions.Length; i++)
        {
            weights_sum += this.decisions[i].GetDecisionWeight();
        }

        // step 2
        int random_num = Random.Range(0, weights_sum);

        // step 3
        for(var i = 0; i < this.decisions.Length; i++)
        {
            if(random_num < this.decisions[i].GetDecisionWeight())
            {
                this.IncreaseWeights(i);
                return this.decisions[i];
            }
            random_num -= this.decisions[i].GetDecisionWeight();
        }
        Debug.Log("<color=red>Error: </color> No decision selected! Fix: check if decision weights are correct");
        return null;
    }

    /// <summary>
    /// Increase the weights of the decisions that weren't selected and decrease the weight of the decision that was selected
    /// </summary>
    /// <param name="decSelectedIdx">The index of the selected decision</param>
    public void IncreaseWeights(int decSelectedIdx)
    {
        for(var i = 0; i < this.decisions.Length; i++)
        {
            if (i == decSelectedIdx)
            {
                this.decisions[i].AddWeight(-5);
            }
            else
            {
                this.decisions[i].AddWeight(5);
            }
        }
    }

    /// <summary>
    /// Add or remove the amount given.
    /// Keeps the opinion between -100 and 100
    /// </summary>
    /// <param name="amount">The number to change the opinion amount by</param>
    public void AddOpinion(int amount)
    {
        this.opinion += amount;

        // Limit opinion to between -100 and 100
        if (this.opinion < -100)
        {
            this.opinion = -100;
        }
        if(this.opinion > 100)
        {
            this.opinion = 100;
        }
    }

    /// <summary>
    /// Check if the opinion is high or low enough to affect the resource effects
    /// </summary>
    /// <param name="decision">The selected decision</param>
    public void CheckOpinionAffect(Decision decision)
    {
        if (this.opinion >= 100)
        {
            // decrease resource costs by 4 and increase gains by 4
            decision.SetOpinionMultiplier(0.25f, 4.0f);
        }
        if (this.opinion >= 50 && this.opinion < 100)
        {
            // decrease resource costs by 2 and increase gains by 2
            decision.SetOpinionMultiplier(0.5f, 2.0f);
        }
        if(this.opinion < 50 && this.opinion > -50)
        {
            // normal
            decision.SetOpinionMultiplier(1.0f, 1.0f);
        }
        if (this.opinion <= -50 && this.opinion > -100)
        {
            // increase resource costs by 2 and decrease gains by 2
            decision.SetOpinionMultiplier(2.0f, 0.5f);
        }
        if (this.opinion <= -100)
        {
            // increase resource costs by 4 and decrease gains by 4
            decision.SetOpinionMultiplier(4.0f, 0.25f);
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
        if (this.weight < 1)
        {
            this.weight = 1;
        }
    }

    /// <summary>
    /// Calculate the character's opinion change to the player's choice
    /// </summary>
    /// <param name="choice">The choice the player selected</param>
    public void ChoiceSelected(Choice choice)
    {
        // get the choice traits.
        Trait[] choiceTraits = choice.GetChoiceTraits();

        // compare each choice trait to each character trait
        for(var i = 0; i < this.traits.Length; i++)
        {
            for(var j = 0; j < choiceTraits.Length; j++)
            {
                // check if the traits are the same
                if (this.traits[i].GetTraitName() == choiceTraits[j].GetTraitName())
                {
                    // this person likes your decision because it lines up with their views!
                    this.AddOpinion(5);
                }

                // check if the traits are opposites
                else if (this.traits[i].GetTraitName() == choiceTraits[j].GetOppositeTrait().GetTraitName())
                {
                    // this person dislikes your decision because it goes against their views!
                    this.AddOpinion(-5);
                }

                // the traits are not related
                else
                {
                    // this person has no strong opinions about your decision!
                }
            }
        }
    }



    // UI FUNCTIONS

    /// <summary>
    /// Setup the UI of all the character traits
    /// </summary>
    /// <param name="prefab">The text box to instantiate and write to</param>
    /// <param name="parent">The parent object to set as the parent</param>
    /// <param name="position">The base position of the text box</param>
    public void SetupUI(TMP_Text prefab, GameObject parent, Vector2 position)
    {   
        for(var i = 0; i < this.traits.Length; i++)
        {
            this.traits[i].SetupUI(prefab, parent, position);
        }
    }

    /// <summary>
    /// Create the tooltips for the traits
    /// </summary>
    /// <param name="tooltips">The list of tooltips created</param>
    /// <param name="prefab">The textbox prefab to instantiate and write the trait description to</param>
    public void SetupTooltip(GameObject[] tooltips, TMP_Text prefab)
    {
        for (var i = 0; i < this.traits.Length; i++)
        {
            this.traits[i].SetupTooltip(tooltips[i], prefab);
        }
    }

    /// <summary>
    /// Destroy the UI of all the character traits and tooltips
    /// </summary>
    public void DestroyUI()
    {
        for(var i = 0; i < this.traits.Length; i++)
        {
            this.traits[i].DestroyUI();   
        }
    }



    // SPRITES

    /// <summary>
    /// Instantiate the sprite, set its parent and position.
    /// </summary>
    public void CreateSprite()
    {
        this.characterSprite = GameObject.Instantiate(this.characterSpritePrefab);
        this.characterSprite.transform.SetParent(this.characterRoot.transform);
        this.characterSprite.transform.localPosition = new Vector2(0, -77);
    }

    /// <summary>
    /// Destroy the sprite
    /// </summary>
    public void DestroySprite()
    {
        GameObject.Destroy(this.characterSprite);
    }
}