using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // VARIABLES

    // Prefabs
    [Header("Prefabs")] //docs.unity3d.com/ScriptReference/HeaderAttribute.html
    [SerializeField] private Button buttonPrefab;
    [SerializeField] private TMP_Text textBoxPrefab;
    [SerializeField] private GameObject tooltipBoxPrefab;

    // UI Windows
    private Canvas canvas;
    private GameObject characterWindow;
    private GameObject resourceWindow;
    private GameObject tooltipWindow;
    private GameObject messageBox;
    private GameObject buttonWindow;
    private GameObject itemWindow;
    private GameObject objectiveWindow;

    // UI Text Elements and Scroll Rects
    private TMP_Text characterNameUI;
    private GameObject characterTraitsUI;
    private TMP_Text characterOpinionUI;
    private TMP_Text decisionNameUI;
    private GameObject decisionDescUI;
    private GameObject choicesUI;
    private TMP_Text dayUI;
    private GameObject resourcesUI;
    private TMP_Text mbUIText;
    private Button mbUIButton;

    private void Start()
    {
        // get the canvas
        canvas = GetComponent<Canvas>();

        // get the UI windows
        this.characterWindow = this.canvas.transform.GetChild(0).gameObject;
        this.resourceWindow = this.canvas.transform.GetChild(1).gameObject;
        this.tooltipWindow = this.canvas.transform.GetChild(2).gameObject;
        this.messageBox = this.canvas.transform.GetChild(3).gameObject;
        this.buttonWindow = this.canvas.transform.GetChild(4).gameObject;
        this.itemWindow = this.canvas.transform.GetChild(5).gameObject;
        this.objectiveWindow = this.canvas.transform.GetChild(6).gameObject;

        // get the UI text elements and scroll rects
        this.characterNameUI = this.characterWindow.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
        this.characterTraitsUI = this.characterWindow.transform.GetChild(1).gameObject;
        this.characterOpinionUI = this.characterWindow.transform.GetChild(2).gameObject.GetComponent<TMP_Text>();
        this.decisionNameUI = this.characterWindow.transform.GetChild(3).gameObject.GetComponent<TMP_Text>();
        this.decisionDescUI = this.characterWindow.transform.GetChild(4).gameObject;
        this.choicesUI = this.characterWindow.transform.GetChild(5).gameObject;
        this.dayUI = this.resourceWindow.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
        this.resourcesUI = this.resourceWindow.transform.GetChild(1).gameObject;
        this.mbUIText = this.messageBox.transform.Find("Result").transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        this.mbUIButton = this.messageBox.transform.Find("Button").GetComponent<Button>();
    }



    // DAY UI

    /// <summary>
    /// Updates the day UI with the current day
    /// </summary>
    /// <param name="day">The current day</param>
    public void UpdateDayUI(int day)
    {
        this.dayUI.text = "Day: " + day;
    }

    /// <summary>
    /// Display an end of day message with the current day and people dealt with on that day
    /// </summary>
    /// <param name="day">Current day</param>
    /// <param name="peopleCount">People dealt with on that day</param>
    /// <returns>The button to add on click functionality to</returns>
    public Button EndOfDayUI(int day, int peopleCount)
    {
        // Set the text
        this.mbUIText.text = "Day " + day + " has ended. You dealt with " + peopleCount + " people";
        this.mbUIButton.GetComponentInChildren<TMP_Text>().text = "Next day...";
        this.mbUIButton.enabled = true;

        // return button to add onclickfunc in game cycle
        return this.mbUIButton;
    }



    // RESOURCES UI //

    /// <summary>
    /// Setup the resources UI
    /// </summary>
    /// <param name="resources">The list of resources</param>
    public void SetupResourcesUI(Resources resources)
    {
        Vector2 position = this.resourcesUI.transform.localPosition;
        resources.SetupUI(this.textBoxPrefab, this.resourceWindow.transform.GetChild(1).gameObject, position);
    }

    /// <summary>
    /// Update the resources UI
    /// </summary>
    /// <param name="resources">The list of resources</param>
    public void UpdateResourcesUI(Resources resources)
    {
        resources.UpdateUI(); 
    }



    // CHARACTERS AND DECISIONS UI

    /// <summary>
    /// Setup the UI for the selected character and decision
    /// </summary>
    /// <param name="character">The selected character</param>
    /// <param name="decision">The selected decision</param>
    public void SetupCharacterDecisionUI(Character character, Decision decision)
    {
        // These are done like this because they are the same amount for each.
        // Instantiate would be inefficient
        // Character name and decision name and description
        this.characterNameUI.text = character.GetCharacterName();
        this.decisionNameUI.text = decision.GetDecisionName();
        this.decisionDescUI.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = decision.GetDecisonDesc();

        // Character opinion
        if (character.GetCharacterOpinion() > 0) { this.characterOpinionUI.color = Color.green; }
        if (character.GetCharacterOpinion() < 0) { this.characterOpinionUI.color = Color.red; }
        if (character.GetCharacterOpinion() == 0) { this.characterOpinionUI.color = Color.yellow; }
        this.characterOpinionUI.text = "Opinion: " + character.GetCharacterOpinion();

        this.characterNameUI.enabled = true;
        this.decisionNameUI.enabled = true;
        this.decisionDescUI.SetActive(true);
        this.characterOpinionUI.enabled = true;

        // Character Traits
        Vector2 position = this.characterTraitsUI.transform.localPosition;
        character.SetupUI(this.textBoxPrefab, this.characterWindow.transform.GetChild(1).GetChild(0).GetChild(0).gameObject, position);

        GameObject[] tooltips = new GameObject[character.GetCharacterTraits().Length];
        for (var i = 0; i < character.GetCharacterTraits().Length; i++)
        {
            tooltips[i] = this.CreateTooltip();
        }
        character.SetupTooltip(tooltips, textBoxPrefab);
    }

    /// <summary>
    /// Destroy the character traits UI and disable the rest of the character/decision UI
    /// </summary>
    /// <param name="character">The character to destroy the UI of</param>
    public void DestroyCharacterDecisionUI(Character character)
    {
        this.characterNameUI.enabled = false;
        this.decisionNameUI.enabled = false;
        this.decisionDescUI.SetActive(false);
        this.characterOpinionUI.enabled = false;
        character.DestroyUI();
    }



    // TOOLTIP UI

    /// <summary>
    /// Create a tooltip
    /// </summary>
    /// <returns>The tooltip</returns>
    public GameObject CreateTooltip()
    {
        GameObject tooltip = GameObject.Instantiate(this.tooltipBoxPrefab);
        tooltip.transform.SetParent(this.canvas.transform); // discussions.unity.com/t/instantiate-as-a-child-of-the-parent/43354
        tooltip.transform.localPosition = this.tooltipWindow.transform.position; // forum.unity.com/threads/how-to-set-child-relative-position-to-its-parent.128705/
        tooltip.SetActive(false);

        return tooltip;
    }



    // CHOICES UI

    /// <summary>
    /// Setup the UI for the choices
    /// </summary>
    /// <param name="decision">The selected decision</param>
    /// <param name="character">The selected character</param>
    /// <returns>The created buttons to add on click functionality to</returns>
    public Button[] SetupChoicesUI(Decision decision, Character character)
    {
        Vector2 position = this.choicesUI.transform.localPosition;
        Button[] buttons = decision.SetupButtonUI(buttonPrefab, this.characterWindow.transform.GetChild(5).GetChild(0).GetChild(0).gameObject, position);

        GameObject[] tooltips = new GameObject[decision.GetDecisionChoices().Length];
        for(var i = 0; i < decision.GetDecisionChoices().Length; i++)
        {
            tooltips[i] = this.CreateTooltip();
        }
        decision.SetupTooltip(tooltips, this.textBoxPrefab, character);

        // return buttons to add onclickfunc in game cycle
        return buttons;

    }

    /// <summary>
    /// Calls the function to enable and disable the choice buttons
    /// </summary>
    /// <param name="decision">The selected decision</param>
    public void EnableDisableButton(Decision decision)
    {
        decision.EnableDisableButton();
    }

    /// <summary>
    /// Destroy the UI for the choices
    /// </summary>
    /// <param name="decision">The selected decision</param>
    public void DestroyChoicesUI(Decision decision)
    {
        // remove buttons and tooltips
        decision.DestroyUI();
    }



    // MESSAGE BOX UI

    /// <summary>
    /// Disable the message UI by removing the listeners and disabling the button
    /// </summary>
    public void DisableMessageUI()
    {
        this.mbUIButton.onClick.RemoveAllListeners();
        this.mbUIButton.enabled = false;
    }



    // RESULTS UI

    /// <summary>
    /// Display the results of the players choice
    /// </summary>
    /// <param name="choiceSelected">The choice the player selected</param>
    /// <returns>The button to add on click functionality to</returns>
    public Button ResultsUI(Choice choiceSelected)
    {
        // Reset text
        this.mbUIText.text = "";

        // Debug purposes :)
        if (choiceSelected.GetChoiceResult().Length == 0)
        {
            Debug.Log("<color=red>Error: </color> No result message found! Fix: check if the result message exists");
        }

        // Add the text
        this.mbUIText.text += choiceSelected.GetChoiceResult();

        // Chance effects added
        ChanceEffect[] choiceChanceEffects = choiceSelected.GetChoiceChanceEffects();
        for (var i = 0; i < choiceChanceEffects.Length; i++)
        {
            if (choiceChanceEffects[i].GetEffectIsSuccess())
            {
                this.mbUIText.text += "<br>" + choiceChanceEffects[i].GetEffectSuccessResult();
                this.mbUIText.text += this.GetResourceEffectText(choiceChanceEffects[i].GetEffectSuccessResourceEffects());
            }
            else
            {
                this.mbUIText.text += "<br>" + choiceChanceEffects[i].GetEffectFailResult();
                this.mbUIText.text += this.GetResourceEffectText(choiceChanceEffects[i].GetEffectFailResourceEffects());
            }
        }

        // Resource effects added
        ResourceEffect[] choiceResourceEffects = choiceSelected.GetChoiceResourceEffects();
        this.mbUIText.text += this.GetResourceEffectText(choiceResourceEffects);

        // Event effects added
        EventEffect[] choiceEventEffects = choiceSelected.GetChoiceEventEffects();
        for (var i = 0; i < choiceEventEffects.Length; i++)
        {
            this.mbUIText.text += "<br>" + choiceEventEffects[i].GetEventName() + " chance has increased by " + choiceEventEffects[i].GetEventAmount();
        }

        this.mbUIButton.GetComponentInChildren<TMP_Text>().text = "Next!";
        this.mbUIButton.enabled = true;

        // return button to add onclickfunc in game cycle
        return this.mbUIButton;
    }

    /// <summary>
    /// Generate the message box resources text from a list of resource effects
    /// </summary>
    /// <param name="resourceEffects">A list of resource effects</param>
    /// <returns>The resource effects text for the message box</returns>
    public string GetResourceEffectText(ResourceEffect[] resourceEffects)
    {
        string resourceEffectString = "";

        for (var i = 0; i < resourceEffects.Length; i++)
        {
            // add resource amount 
            resourceEffectString += "<br>" + resourceEffects[i].GetMultipliedAmount();

            // add resource name
            resourceEffectString += " " + resourceEffects[i].GetEffectResourceName();

            // add the appropriate word after
            if (resourceEffects[i].GetMultipliedAmount() >= 0)
            {
                resourceEffectString += " gained.";
            }
            else
            {
                resourceEffectString += " lost.";
            }
        }
        return resourceEffectString;
    }



    // EVENT UI

    /// <summary>
    /// Display a message showing an event has happened
    /// </summary>
    /// <param name="eventsHappened">All the events that have happened</param>
    /// <returns>The button to add on click functionality to</returns>
    public Button EventsUI(Event[] eventsHappened)
    {
        // If any events have actually happened
        bool eventsHaveHappened = false;

        // Set the text
        for (var i = 0; i < eventsHappened.Length; i++)
        {
            if (eventsHappened[i] != null)
            {
                eventsHaveHappened = true;
                this.mbUIText.text = eventsHappened[i].GetEventName();
                this.mbUIText.text += "<br>" + eventsHappened[i].GetEventDesc();

                // resource effects added
                ResourceEffect[] eventResourceEffects = eventsHappened[i].GetEventResourceEffects();
                this.mbUIText.text += "<br>" + this.GetResourceEffectText(eventResourceEffects);
            }
        }

        // If any events have actually happened then enable the message box.
        if (eventsHaveHappened) {
            this.mbUIButton.GetComponentInChildren<TMP_Text>().text = "Oh dear!";
            this.mbUIButton.enabled = true;

            // return button to add onclickfunc in game cycle
            return this.mbUIButton;
        }
        // Otherwise return null
        else { return null; }
    }



    // BUTTONS UI

    /// <summary>
    /// Setup the buttons UI
    /// </summary>
    /// <returns>The buttons to add on click functionality to</returns>
    public Button[] SetupButtonUI()
    {
        Button[] buttons = new Button[this.buttonWindow.transform.childCount-1];

        for (var i = 0; i < this.buttonWindow.transform.childCount-1; i++)
        {
            GameObject go = this.buttonWindow.transform.GetChild(i).gameObject;
            buttons[i] = go.GetComponent<Button>();
        }

        // return buttons to add onclickfunc in game cycle
        return buttons;
    }



    // ITEMS UI

    /// <summary>
    /// Setup the items UI
    /// </summary>
    /// <param name="items">The list of items</param>
    /// <returns>The button to add on click functionality to</returns>
    public Button SetupItemsUI(Items items)
    {
        Vector2 position = this.itemWindow.transform.GetChild(1).transform.localPosition;
        items.SetupUI(this.textBoxPrefab, this.itemWindow.transform.GetChild(1).GetChild(0).GetChild(0).gameObject, position);

        return this.itemWindow.transform.GetChild(2).GetComponent<Button>();
    }

    /// <summary>
    /// Update the items UI
    /// </summary>
    /// <param name="items">The list of items</param>
    public void UpdateItemsUI(Items items)
    {
        items.UpdateUI();
    }



    // OBJECTIVES UI

    /// Setup the objectives UI
    /// </summary>
    /// <param name="objectives">The list of items</param>
    /// <returns>The button to add on click functionality to</returns>
    public Button SetupObjectivesUI(Objectives objectives)
    {
        Vector2 position = this.objectiveWindow.transform.GetChild(1).transform.localPosition;
        objectives.SetupUI(this.textBoxPrefab, this.objectiveWindow.transform.GetChild(1).GetChild(0).GetChild(0).gameObject, position);

        return this.objectiveWindow.transform.GetChild(2).GetComponent<Button>();
    }

    /// <summary>
    /// Update the objectives UI
    /// </summary>
    /// <param name="objectives">The list of resources</param>
    public void UpdateObjectivesUI(Objectives objectives)
    {
        objectives.UpdateUI();
    }
}
