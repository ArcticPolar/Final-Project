using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class Resource
{
    // VARIABLES

    [SerializeField] private string name;
    [SerializeField] private int amount;

    // stores the UI for this resource
    private TMP_Text resourceUI;



    // GET FUNCTIONS

    /// <summary>
    /// Get the name of the resource
    /// </summary>
    /// <returns>The name of the resource</returns>
    public string GetResourceName()
    {
        return this.name;
    }

    /// <summary>
    /// Get the amount of the resource
    /// </summary>
    /// <returns>The amount of the resource</returns>
    public int GetResourceAmount()
    {
        return this.amount;
    }



    /// <summary>
    /// Add or remove the amount given
    /// </summary>
    /// <param name="amount">The number to change the resource amount by</param>
    public void AddResource(int amount)
    {
        this.amount += amount;
    }



    // UI FUNCTIONS //

    /// <summary>
    /// Setup the UI of the resource
    /// </summary>
    /// <param name="prefab">The text box to instantiate and write to</param>
    /// <param name="parent">The parent object to set as the parent</param>
    /// <param name="position">The position of the text box</param>
    public void SetupUI(TMP_Text prefab, GameObject parent, Vector2 position)
    {
        this.resourceUI = GameObject.Instantiate(prefab);
        this.resourceUI.transform.SetParent(parent.transform);
        this.resourceUI.transform.localPosition = position;
        this.resourceUI.fontSize = 16;
    }

    /// <summary>
    /// Update the UI of the resource
    /// </summary>
    public void UpdateUI()
    {
        this.resourceUI.text = this.name + ": " + this.amount;
    }
}
