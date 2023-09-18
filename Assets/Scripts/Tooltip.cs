using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // VARIABLES

    private GameObject tooltip;



    // GET FUNCTIONS

    /// <summary>
    /// Get the tooltip
    /// </summary>
    /// <returns>The tooltip Game Object</returns>
    public GameObject GetTooltip()
    {
        return this.tooltip;
    }



    // SET FUNCTIONS

    /// <summary>
    /// Set the tooltip Game Object to the given Game Object
    /// </summary>
    /// <param name="tooltip">Game Object to set the tooltip Game Object to</param>
    public void SetTooltip(GameObject tooltip)
    {
        this.tooltip = tooltip;
    }



    //docs.unity3d.com/2019.1/Documentation/ScriptReference/UI.Selectable.OnPointerEnter.html
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (this.tooltip != null)
        {
            this.tooltip.SetActive(true);
        }
    }

    //docs.unity3d.com/2019.1/Documentation/ScriptReference/UI.Selectable.OnPointerExit.html
    public void OnPointerExit(PointerEventData eventData)
    {
        if (this.tooltip != null)
        {
            this.tooltip.SetActive(false);
        }
    }
}
