using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Resources : MonoBehaviour
{
    // VARIABLES

    [SerializeField] private Resource[] resources;



    // GET FUNCTIONS

    /// <summary>
    /// Get all the resources
    /// </summary>
    /// <returns>The list of resources</returns>
    public Resource[] GetResources()
    {
        return this.resources;
    }

    /// <summary>
    /// Iterate through the resources to find the given resource name
    /// </summary>
    /// <param name="resourceName">The name of the resource being searched for</param>
    /// <returns>The requested resource or null if the resource isn't found (for debug purposes)</returns>
    public Resource GetResourceByName(string resourceName)
    {
        for (var i = 0; i < this.resources.Length; i++)
        {
            if (this.resources[i].GetResourceName() == resourceName)
            {
                Resource resource = this.resources[i];
                return resource;
            }
        }
        Debug.Log("<color=red>Error: </color> Resource " + resourceName + " not found! Fix: check if the resource name typed in is correct");
        return null;
    }



    // UI FUNCTIONS //

    /// <summary>
    /// Setup the UI of all the resources
    /// </summary>
    /// <param name="prefab">The text box to instantiate and write to</param>
    /// <param name="parent">The parent object to set as the parent</param>
    /// <param name="position">The base position of the text box</param>
    public void SetupUI(TMP_Text prefab, GameObject parent, Vector2 position)
    {
        for(var i = 0; i < this.resources.Length; i++)
        {
            this.resources[i].SetupUI(prefab, parent, position);
        }
    }

    /// <summary>
    /// Update the UI of all the resources
    /// </summary>
    public void UpdateUI()
    {
        for (var i = 0; i < this.resources.Length; i++)
        {
            this.resources[i].UpdateUI();
        }
    }
}
