using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Objectives : MonoBehaviour
{
    // VARIABLES

    [SerializeField] private Objective[] objectives;



    /// <summary>
    /// Call the function in the objectives to link the resources and rewards (items)
    /// </summary>
    /// <param name="resources">The list of resources</param>
    /// <param name="items">The list of events</param>
    public void LinkResources(Resources resources, Items items)
    {
        for(var i = 0; i < this.objectives.Length; i++)
        {
            this.objectives[i].LinkResource(resources);
            this.objectives[i].LinkReward(items);
        }
    }

    /// <summary>
    /// Call the function in the objectives to check if it has been completed
    /// </summary>
    /// <param name="days">The number of days played</param>
    /// <param name="people">The number of people dealt with</param>
    public void CheckObjectives(int days, int people)
    {
        for(var i = 0; i < this.objectives.Length; i++)
        {
            this.objectives[i].CheckObjective(days, people);
        }
    }



    // UI FUNCTIONS //

    /// <summary>
    /// Setup the UI of all the items
    /// </summary>
    /// <param name="prefab">The text box to instantiate and write to</param>
    /// <param name="parent">The parent object to set as the parent</param>
    /// <param name="position">The base position of the text box</param>
    public void SetupUI(TMP_Text prefab, GameObject parent, Vector2 position)
    {
        for (var i = 0; i < this.objectives.Length; i++)
        {
            this.objectives[i].SetupUI(prefab, parent, position);
        }
    }

    /// <summary>
    /// Update the UI of all the objectives
    /// </summary>
    public void UpdateUI()
    {
        for (var i = 0; i < this.objectives.Length; i++)
        {
            this.objectives[i].UpdateUI();
        }
    }
}
