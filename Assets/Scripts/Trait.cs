using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class Trait
{
    // VARIABLES

    [SerializeField] private string name;
    [SerializeField] private string desc;
    [SerializeField] private string oppositeTraitName;

    private Trait oppositeTrait;

    // for when traits are created for the choice UI
    private TMP_Text traitUI;
    private GameObject tooltip;
    private TMP_Text tooltipText;



    /// GET FUNCTIONS

    /// <summary>
    /// Get the name of the trait
    /// </summary>
    /// <returns>The name of the trait</returns>
    public string GetTraitName()
    {
        return this.name;
    }

    /// <summary>
    /// Get the description of the trait
    /// </summary>
    /// <returns>The description of the trait</returns>
    public string GetTraitDesc()
    {
        return this.desc;
    }

    /// <summary>
    /// Get the given opposite trait name of the trait
    /// </summary>
    /// <returns>The name of the opposite trait</returns>
    public string GetGivenOppositeTraitName()
    {
        return this.oppositeTraitName;
    }

    /// <summary>
    /// Get the opposite trait of the trait
    /// </summary>
    /// <returns>The opposite trait</returns>
    public Trait GetOppositeTrait()
    {
        return this.oppositeTrait;
    }



    /// <summary>
    /// Assigns the opposite trait to the given trait.
    /// This has to be done after all traits have been initialised
    /// </summary>
    /// <param name="oppositeTrait">The opposite trait to be assigned</param>
    public void AddOppositeTrait(Trait oppositeTrait)
    {
        this.oppositeTrait = oppositeTrait;
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
        this.traitUI = GameObject.Instantiate(prefab);
        this.traitUI.transform.SetParent(parent.transform, false);
        this.traitUI.transform.localPosition = position;
        this.traitUI.text = this.name;
        this.traitUI.alignment = TextAlignmentOptions.Center;
    }

    /// <summary>
    /// Setup the UI of the trait tooltip
    /// </summary>
    /// <param name="createdTooltip">The tooltip created for this trait</param>
    /// <param name="prefab">The text box to instantiate and write to</param>
    public void SetupTooltip(GameObject createdTooltip, TMP_Text prefab)
    {
        this.tooltip = createdTooltip;

        Vector2 ttGeneric = createdTooltip.transform.GetChild(4).localPosition;

        // attach it to script that runs the tooltip check
        Tooltip tooltip = this.traitUI.GetComponent<Tooltip>();
        tooltip.SetTooltip(this.tooltip);

        this.tooltipText = GameObject.Instantiate(prefab);
        this.tooltipText.transform.SetParent(this.tooltip.transform);
        this.tooltipText.transform.localPosition = ttGeneric;
        this.tooltipText.text = this.desc;
    }

    /// <summary>
    /// Destroy the UI of the tooltip
    /// </summary>
    public void DestroyUI()
    {
        GameObject.Destroy(this.tooltipText.gameObject);
        GameObject.Destroy(this.tooltip.gameObject);
        GameObject.Destroy(this.traitUI.gameObject);
    }
}
