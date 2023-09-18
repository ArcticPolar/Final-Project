using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Item
{
    // The type of the item
    public enum Type
    {
        daily, // effect happens at the end of each day
        constant // effect happens at all times
    }

    // VARIABLES

    [SerializeField] private string name;
    [SerializeField] private string desc;
    [SerializeField] private Type type;

    private bool playerHas = false;
    private Objective unlockObjective;

    private TMP_Text itemUI;



    // GET FUNCTIONS

    /// <summary>
    /// Get the name of the item
    /// </summary>
    /// <returns>The name of the item</returns>
    public string GetItemName()
    {
        return this.name;
    }

    /// <summary>
    /// Get the description of the item
    /// </summary>
    /// <returns>The description of the item</returns>
    public string GetItemDesc()
    {
        return this.desc;
    }

    /// <summary>
    /// Get if the player has the item
    /// </summary>
    /// <returns>Whether the player has the item</returns>
    public bool GetPlayerHas()
    {
        return this.playerHas;
    }

    /// <summary>
    /// Get the item type
    /// </summary>
    /// <returns>The item type</returns>
    public Type GetItemType()
    {
        return this.type;
    }



    // SET FUNCTIONS

    /// <summary>
    /// Set if the player has the item
    /// </summary>
    /// <param name="unlocked">Is the item unlocked</param>
    public void SetPlayerHas(bool unlocked)
    {
        this.playerHas = unlocked;
    }

    /// <summary>
    /// Set the unlock objective for the item
    /// </summary>
    /// <param name="objective">The objective for unlocking this item</param>
    public void SetUnlockObjective(Objective objective)
    {
        this.unlockObjective = objective;
    }



    /// <summary>
    /// Apply the item effect for daily items
    /// </summary>
    public virtual void ApplyItemEffect()
    {

    }

    /// <summary>
    /// Apply the item effect for constant items
    /// </summary>
    /// <param name="charSelected">The character selected</param>
    /// <param name="decSelected">The decision selected</param>
    /// <param name="isApply">Whether the effect is being applied or unapplied (so that it doesn't stack every single time)</param>
    public virtual void ApplyItemEffect(Character charSelected, Decision decSelected, bool isApply)
    {

    }



    // UI FUNCTIONS

    /// <summary>
    /// Setup the UI of the item
    /// </summary>
    /// <param name="prefab">The text box to instantiate and write to</param>
    /// <param name="parent">The window object to set as the parent</param>
    /// <param name="position">The position of the text box</param>
    public void SetupUI(TMP_Text prefab, GameObject parent, Vector2 position)
    {
        this.itemUI = GameObject.Instantiate(prefab);
        this.itemUI.transform.SetParent(parent.transform, false);
        this.itemUI.transform.localPosition = position;
    }

    /// <summary>
    /// Update the UI of the item
    /// </summary>
    public void UpdateUI()
    {
        if (this.playerHas)
        {
            this.itemUI.text = this.name + "<br>" + this.desc;
        }
        else
        {
            this.itemUI.text = "Locked! <br>Complete: " + this.unlockObjective.GetObjectiveName();
        }      
    }
}
