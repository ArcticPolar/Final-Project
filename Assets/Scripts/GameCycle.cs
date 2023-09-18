using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCycle : MonoBehaviour
{
    // VARIABLES
    
    // Systems
    [SerializeField] private Traits traitSystem;
    [SerializeField] private Characters characterSystem;
    [SerializeField] private Resources resourceSystem;
    [SerializeField] private Events eventSystem;
    [SerializeField] private Items itemSystem;
    [SerializeField] private Objectives objectiveSystem;

    // Managers
    [SerializeField] private UIManager UIManager;
    [SerializeField] private AnimationManager animManager;
    [SerializeField] private AudioManager audioManager;

    // Keep important count
    private int day;
    private int peopleCountTotal;

    private int charPerDay = 5;

    // Start is called before the first frame update
    void Start()
    {
        // Setup the traits UI (Link opposite traits)
        this.traitSystem.AddOppositeTraits();

        // Setup the characters UI
        this.characterSystem.LinkTraitsAndResources(this.traitSystem, this.resourceSystem, this.eventSystem);

        // Setup the events UI
        this.eventSystem.LinkEventResource(this.resourceSystem);

        // Setup the items and objectives UI
        this.itemSystem.CombineItems();
        this.objectiveSystem.LinkResources(this.resourceSystem, this.itemSystem);

        // Setup the resources UI
        this.UIManager.SetupResourcesUI(this.resourceSystem);
        this.UIManager.UpdateResourcesUI(this.resourceSystem);

        // Setup the buttons UI
        Button[] buttons = this.UIManager.SetupButtonUI();
        buttons[0].onClick.AddListener(delegate { OnClickOpenItem(); });
        buttons[1].onClick.AddListener(delegate { OnClickOpenObjective();  });

        // Setup the items ui and set close button
        Button itemCloseButton = this.UIManager.SetupItemsUI(this.itemSystem);
        itemCloseButton.onClick.AddListener(delegate { OnClickCloseItem(); });
        this.UIManager.UpdateItemsUI(this.itemSystem);

        // Setup the objectives ui and set close button
        Button objectiveCloseButton = this.UIManager.SetupObjectivesUI(this.objectiveSystem);
        objectiveCloseButton.onClick.AddListener(delegate { OnClickCloseObjective(); });
        this.UIManager.UpdateObjectivesUI(this.objectiveSystem);

        // Set the start day to 1 and setup the day UI
        this.day = 1;
        this.UIManager.UpdateDayUI(this.day);

        // Setup the day with charPerDay characters
        SetupDay(charPerDay);
    }



    // SETUP

    // count is the number of characters selected.
    /// <summary>
    /// Setup up the day with count characters
    /// </summary>
    /// <param name="count">The amount of characters for that day</param>
    void SetupDay(int count)
    {
        // Select the characters for the day
        Character[] selectedCharacters = SelectCharacters(count);

        // Move in the king
        this.audioManager.PlayFanfare();
        this.animManager.RunKingAnimIn();

        StartCoroutine(WaitUntilKingWalkIn(5, selectedCharacters));
    }

    /// <summary>
    /// Wait until the king has walked in
    /// </summary>
    /// <param name="time">The time to wait</param>
    /// <param name="selectedCharacters">The selected characters</param>
    /// <returns></returns>
    private IEnumerator WaitUntilKingWalkIn(float time, Character[] selectedCharacters)
    {
        yield return new WaitForSeconds(time);

        // start the music
        this.audioManager.PlayMusic();

        SetupInteraction(selectedCharacters, 0);
    }

    /// <summary>
    /// Select count number of characters for the day
    /// </summary>
    /// <param name="count">The number of characters to select</param>
    /// <returns></returns>
    Character[] SelectCharacters(int count)
    {
        Character[] selectedCharacters = new Character[count];

        // select x characters
        for (var i = 0; i < count; i++)
        {
            // select a character
            Character charSelected = this.characterSystem.SelectCharacter();

            // initialise as not found
            bool found = false;

            // check already selected characters
            for (var j = 0; j < i; j++)
            {
                // if the character has already been selected set to match
                if (charSelected.GetCharacterName() == selectedCharacters[j].GetCharacterName())
                {
                    found = true;
                }
            }

            // if not already selected, add it
            if (found == false)
            {
                selectedCharacters[i] = charSelected;
            }
            // else re select
            else
            {
                i -= 1;
            }
        }
        return selectedCharacters;
    }



    // INTERACTION

    /// <summary>
    /// Start the interaction with the selected character at the given index
    /// </summary>
    /// <param name="selectedCharacters">The list of selected characters</param>
    /// <param name="index">The index of the current character</param>
    void SetupInteraction(Character[] selectedCharacters, int index)
    {
        this.peopleCountTotal += 1;

        // Choose the character at the given index.
        Character charSelected = selectedCharacters[index];

        // instantiate the prefab
        charSelected.CreateSprite();

        // Select a decision
        Decision decSelected = charSelected.SelectDecision();

        // check if character opinion affects decision
        charSelected.CheckOpinionAffect(decSelected);

        // enable any item effects
        this.itemSystem.RunEffects(charSelected, decSelected, true);

        // Update the character and decision UI
        this.UIManager.SetupCharacterDecisionUI(charSelected, decSelected);

        // Setup the buttons
        Button[] buttons = this.UIManager.SetupChoicesUI(decSelected, charSelected);
        for (var i = 0; i < decSelected.GetDecisionChoices().Length; i++)
        {
            int tempVar = i;
            buttons[i].onClick.AddListener(delegate { OnClickFunc(decSelected, decSelected.GetDecisionChoices()[tempVar], charSelected, selectedCharacters, index); });
        }

        // start buttons disabled
        this.UIManager.EnableDisableButton(decSelected);

        // run anim of character walking in
        // run anim of character ui appearing
        this.animManager.RunChoiceAnimDown();
        this.animManager.RunCharAnimIn();

        StartCoroutine(WaitUntilSetupAnimDone(5, decSelected));
    }

    /// <summary>
    /// Wait until the character has walked in
    /// </summary>
    /// <param name="time">The time to wait</param>
    /// <param name="decSelected">The selected decision</param>
    /// <returns></returns>
    private IEnumerator WaitUntilSetupAnimDone(int time, Decision decSelected)
    {
        yield return new WaitForSeconds(time);

        // enable buttons
        this.UIManager.EnableDisableButton(decSelected);
    }

    /// <summary>
    /// Function is called when a choice is clicked by the player
    /// </summary>
    /// <param name="decSelected">The selected decision</param>
    /// <param name="choiceSelected">The player selected choice</param>
    /// <param name="charSelected">The selected character</param>
    /// <param name="selectedCharacters">The selected characters</param>
    /// <param name="index">The index of the current position in selected characters</param>
    void OnClickFunc(Decision decSelected, Choice choiceSelected, Character charSelected, Character[] selectedCharacters, int index)
    {
        // Button press sound
        this.audioManager.PlayButtonPress();

        EndInteraction(decSelected, choiceSelected, charSelected, selectedCharacters, index);
    }

    /// <summary>
    /// End the interaction with the selected character at the given index
    /// </summary>
    /// <param name="decSelected">The selected decision</param>
    /// <param name="choiceSelected">The player selected choice</param>
    /// <param name="charSelected">The selected character</param>
    /// <param name="selectedCharacters">The selected characters</param>
    /// <param name="index">The index of the current position in selected characters</param>
    void EndInteraction(Decision decSelected, Choice choiceSelected, Character charSelected, Character[] selectedCharacters, int index)
    {
        // calculate the results
        CalculateResults(choiceSelected, charSelected);

        // disable any added item effects
        this.itemSystem.RunEffects(charSelected, decSelected, false);

        // update the UI
        this.UIManager.UpdateResourcesUI(this.resourceSystem);

        // run anim of character leaving
        // run anim of character ui disappearing
        this.animManager.RunChoiceAnimUp();
        this.animManager.RunCharAnimOut();

        //disable buttons
        this.UIManager.EnableDisableButton(decSelected);

        // run anim for person leaving and UI disappearing
        StartCoroutine(WaitUntilEndAnimDone(5, charSelected, decSelected, choiceSelected, selectedCharacters, index));
    }



    // RESULTS

    /// <summary>
    /// Calculate the results of the selected choice
    /// </summary>
    /// <param name="choiceSelected">The player selected choice</param>
    /// <param name="charSelected">The selected character</param>
    void CalculateResults(Choice choiceSelected, Character charSelected)
    {
        // calculate results
        // get the resource effects
        ResourceEffect[] resourceEffects = choiceSelected.GetChoiceResourceEffects();
        // apply the resource effect(s) of the choice
        for (var i = 0; i < resourceEffects.Length; i++) { resourceEffects[i].ApplyEffect(); }

        // get the event effects
        EventEffect[] eventEffects = choiceSelected.GetChoiceEventEffects();
        // apply the event effect(s) of the choice
        for (var i = 0; i < eventEffects.Length; i++) { eventEffects[i].ApplyEffect(); }

        // get the chance effects
        ChanceEffect[] chanceEffects = choiceSelected.GetChoiceChanceEffects();
        // apply the chance effect(s) of the choice
        for (var i = 0; i < chanceEffects.Length; i++) { chanceEffects[i].ApplyEffect(); }

        // get the opinion effect
        OpinionEffect opinionEffect = choiceSelected.GetOpinionEffect();
        // apply the opinion effect of the choice
        charSelected.AddOpinion(opinionEffect.GetOpinionAmount());

        // originally affects all the selected characters for the day - but this was difficult to show to the player,
        // so now only affects the current one.
        // go through the characters in the court and their opinion of your choice
        //for (var i = 0; i < selectedCharacters.Length; i++) { selectedCharacters[i].ChoiceSelected(choiceSelected); }

        // update the characters opinion of your choice
        charSelected.ChoiceSelected(choiceSelected);
    }

    /// <summary>
    /// Wait until the character has walked out
    /// </summary>
    /// <param name="time">The time to wait</param>
    /// <param name="charSelected">The selected character</param>
    /// <param name="decSelected">The selected decision</param>
    /// <param name="choiceSelected">The player selected choice</param>
    /// <param name="selectedCharacters">The selected characters</param>
    /// <param name="index">The index of the current position in selected characters</param>
    /// <returns></returns>
    private IEnumerator WaitUntilEndAnimDone(int time, Character charSelected, Decision decSelected, Choice choiceSelected, Character[] selectedCharacters, int index)
    {
        yield return new WaitForSeconds(time);

        // destroy buttons and UI
        this.UIManager.DestroyCharacterDecisionUI(charSelected);
        this.UIManager.DestroyChoicesUI(decSelected);

        // destroy the sprite
        charSelected.DestroySprite();

        // show the results of the choice (message box)
        this.animManager.RunMessageAnimUp();
        this.audioManager.PlayResults();

        Button button = this.UIManager.ResultsUI(choiceSelected);
        button.onClick.AddListener(delegate { OnClickResults(selectedCharacters, index); });
    }

    /// <summary>
    /// Function is called when close results is clicked by the player
    /// </summary>
    /// <param name="selectedCharacters">The selected characters</param>
    /// <param name="index">The index of the current position in selected characters</param>
    void OnClickResults(Character[] selectedCharacters, int index)
    {
        // Button press sound
        this.audioManager.PlayButtonPress();

        // Message box anim down
        this.UIManager.DisableMessageUI();
        this.animManager.RunMessageAnimDown();

        StartCoroutine(WaitUntilResultsDone(2, selectedCharacters, index));
    }

    /// <summary>
    /// Wait until the results message box has moved down
    /// </summary>
    /// <param name="time">The time to wait</param>
    /// <param name="selectedCharacters">The selected characters</param>
    /// <param name="index">The index of the current position in selected characters</param>
    /// <returns></returns>
    private IEnumerator WaitUntilResultsDone(int time, Character[] selectedCharacters, int index)
    {
        yield return new WaitForSeconds(time);

        // increment index to point to the next character
        index += 1;

        // if the index is greater than the number of characters selected
        if (index >= selectedCharacters.Length)
        {
            // end the day.
            this.animManager.RunKingAnimOut();

            StartCoroutine(WaitUntilKingWalkOut(5, selectedCharacters));
        }
        else
        {
            // else move on to the next character
            SetupInteraction(selectedCharacters, index);
        }
    }



    // END OF DAY

    /// <summary>
    /// Wait until the king has walked out
    /// </summary>
    /// <param name="time">The time to wait</param>
    /// <param name="selectedCharacters">The selected characters</param>
    /// <returns></returns>
    private IEnumerator WaitUntilKingWalkOut(int time, Character[] selectedCharacters)
    {
        yield return new WaitForSeconds(time);
        
        // check if any events have happened
        CheckEvents(selectedCharacters.Length);
    }

    /// <summary>
    /// Check if any events have happened
    /// </summary>
    /// <param name="peopleCount">The people dealt with today</param>
    void CheckEvents(int peopleCount)
    {
        // events
        Event[] eventsHappened = this.eventSystem.CheckEvents();

        // get the button
        Button button = this.UIManager.EventsUI(eventsHappened);

        // if no events have happened then don't display the message box
        if(button != null)
        {
            this.animManager.RunMessageAnimUp();

            button.onClick.AddListener(delegate { OnClickEvent(peopleCount); });
        }
        // just move onto the next step
        else { EndDay(peopleCount); }
    }

    /// <summary>
    /// Function is called when close event is clicked by the player
    /// </summary>
    /// <param name="peopleCount">The people dealt with today</param>
    void OnClickEvent(int peopleCount)
    {
        // Button press sound
        this.audioManager.PlayButtonPress();

        this.UIManager.DisableMessageUI();
        this.animManager.RunMessageAnimDown();

        StartCoroutine(WaitUntilEventsDone(2, peopleCount));
    }

    /// <summary>
    /// Wait until the events message box has moved down
    /// </summary>
    /// <param name="time">The time to wait</param>
    /// <param name="peopleCount">The people dealt with today</param>
    /// <returns></returns>
    private IEnumerator WaitUntilEventsDone(int time, int peopleCount)
    {
        yield return new WaitForSeconds(time);

        EndDay(peopleCount);
    }

    /// <summary>
    /// End of the day
    /// </summary>
    /// <param name="peopleCount">The people dealt with today</param>
    void EndDay(int peopleCount)
    {
        // screen fade black
        this.animManager.RunNightAnimDark();

        // run daily items
        this.itemSystem.RunEffects();

        // stop the music
        this.audioManager.StopMusic();

        // show message
        this.animManager.RunMessageAnimUp();
        this.audioManager.PlayDayEnd();

        Button button = UIManager.EndOfDayUI(this.day, peopleCount);
        button.onClick.AddListener(delegate { OnClickEndDay(); });
    }

    /// <summary>
    /// Function is called when close end of day is clicked by the player
    /// </summary>
    void OnClickEndDay()
    {
        // Button press sound
        this.audioManager.PlayButtonPress();

        this.UIManager.DisableMessageUI();
        this.animManager.RunMessageAnimDown();

        // Increment the day by 1 and update the day UI
        this.day += 1;
        this.UIManager.UpdateDayUI(this.day);

        // has the player reached any objectives at the end of the day
        this.objectiveSystem.CheckObjectives(this.day, this.peopleCountTotal);

        // Update the items and objectives UI
        this.UIManager.UpdateItemsUI(this.itemSystem);
        this.UIManager.UpdateObjectivesUI(this.objectiveSystem);

        StartCoroutine(WaitUntilEndDayDone(2));
    }

    /// <summary>
    /// Wait until the end of day message box has moved down
    /// </summary>
    /// <param name="time">The time to wait</param>
    /// <returns></returns>
    private IEnumerator WaitUntilEndDayDone(int time)
    {
        yield return new WaitForSeconds(time);

        // screen fade back
        this.animManager.RunNightAnimLight();

        // Setup the day with charPerDay characters
        SetupDay(this.charPerDay);
    }



    // ITEMS AND OBJECTIVES

    /// <summary>
    /// Function is called when open items is clicked by the player
    /// </summary>
    void OnClickOpenItem()
    {
        OnClickCloseObjective();

        // Button press sound
        this.audioManager.PlayButtonPress();

        // animation for item menu going down
        this.animManager.RunItemsAnimDown();
    }

    /// <summary>
    /// Function is called when close items is clicked by the player
    /// </summary>
    void OnClickCloseItem()
    {
        // Button press sound
        this.audioManager.PlayButtonPress();

        // animation for item menu going up
        this.animManager.RunItemsAnimUp();
    }

    /// <summary>
    /// Function is called when open objectives is clicked by the player
    /// </summary>
    void OnClickOpenObjective()
    {
        OnClickCloseItem();

        // Button press sound
        this.audioManager.PlayButtonPress();

        // animation for objective menu going down
        this.animManager.RunObjectivesAnimDown();
    }

    /// <summary>
    /// Function is called when close objectives is clicked by the player
    /// </summary>
    void OnClickCloseObjective()
    {
        // Button press sound
        this.audioManager.PlayButtonPress();

        // animation for objective menu going up
        this.animManager.RunObjectivesAnimUp();
    }
}
