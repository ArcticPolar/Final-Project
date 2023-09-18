using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChanceEffect
{
    // VARIABLES

    [SerializeField] private string successResult;
    [SerializeField] private string failResult;
    [SerializeField] private int chance;
    [SerializeField] private ResourceEffect[] successResourceEffects;
    [SerializeField] private ResourceEffect[] failResourceEffects;

    private bool isSuccess;



    // GET FUNCTIONS

    /// <summary>
    /// Get the success result text
    /// </summary>
    /// <returns>The success result text</returns>
    public string GetEffectSuccessResult()
    {
        return this.successResult;
    }

    /// <summary>
    /// Get the fail result text
    /// </summary>
    /// <returns>The fail result text</returns>
    public string GetEffectFailResult()
    {
        return this.failResult;
    }

    /// <summary>
    /// Get the chance of the effect being a success
    /// </summary>
    /// <returns>The chance of the effect being a success</returns>
    public int GetEffectChance()
    {
        return this.chance;
    }

    /// <summary>
    /// Get if the chance result was a success or fail
    /// </summary>
    /// <returns>Whether the chance result was a success. True = success, false = fail</returns>
    public bool GetEffectIsSuccess()
    {
        return this.isSuccess;
    }

    /// <summary>
    /// Get the success resource effects
    /// </summary>
    /// <returns>The success resource effects</returns>
    public ResourceEffect[] GetEffectSuccessResourceEffects()
    {
        return this.successResourceEffects;
    }

    /// <summary>
    /// Get the fail resource effects
    /// </summary>
    /// <returns>The fail resource effects</returns>
    public ResourceEffect[] GetEffectFailResourceEffects()
    {
        return this.failResourceEffects;
    }



    /// <summary>
    /// Links the success and fail resources to the corresponding resources in the given resources list
    /// </summary>
    /// <param name="resources">The list of resources</param>
    public void LinkResource(Resources resources)
    {
        for (var i = 0; i < this.successResourceEffects.Length; i++)
        {
            this.successResourceEffects[i].LinkResource(resources);
        }
        for (var i = 0; i < this.failResourceEffects.Length; i++)
        {
            this.failResourceEffects[i].LinkResource(resources);
        }
    }

    /// <summary>
    /// Apply the effect.
    /// This calculates a random number and if it is less than the chance then the success effect happens, otherwise the fail effect happens
    /// </summary>
    public void ApplyEffect()
    {
        int random_num = Random.Range(0, 100);

        if (random_num < this.chance)
        {
            // the success effect happens
            this.isSuccess = true;

            for (var i = 0; i < this.successResourceEffects.Length; i++)
            {
                // set the multiplier to be a default 1, 1
                this.successResourceEffects[i].SetOpinionMultiplier(1.0f, 1.0f);
                this.successResourceEffects[i].ApplyEffect();
            }
        }
        else
        {
            // the failure effect happens
            this.isSuccess = false;

            for (var i = 0; i < this.failResourceEffects.Length; i++)
            {
                // set the multiplier to be a default 1, 1
                this.failResourceEffects[i].SetOpinionMultiplier(1.0f, 1.0f);
                this.failResourceEffects[i].ApplyEffect();
            }
        }
    }
}
