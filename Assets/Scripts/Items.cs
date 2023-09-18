using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class Items : MonoBehaviour
{
    // VARIABLES
    [SerializeField] private ItemDaily[] itemsDaily;
    [SerializeField] private ItemConstant[] itemsConstant;

    private Item[] items;



    // GET FUNCTIONS

    /// <summary>
    /// Get all the items
    /// </summary>
    /// <returns>All the items</returns>
    public Item[] GetItems()
    {
        return this.items;
    }

    /// <summary>
    /// Iterate through the items to find the given item name
    /// </summary>
    /// <param name="itemName">The name of the item being searched for</param>
    /// <returns>The requested item or null if the item isn't found (for debug purposes)</returns>
    public Item GetItemByName(string itemName)
    {
        for (var i = 0; i < this.items.Length; i++)
        {
            if (this.items[i].GetItemName() == itemName)
            {
                Item item = this.items[i];
                return item;
            }
        }
        Debug.Log("<color=red>Error: </color> Item " + itemName + " not found! Fix: check if the item name typed in is correct");
        return null;
    }



    /// <summary>
    /// Combine the items from the two inspector arrays.
    /// They both share the same parent class - encapsulation
    /// </summary>
    public void CombineItems()
    {
        // The lengths of the lists
        int lenDaily = this.itemsDaily.Length;
        int lenConstant = this.itemsConstant.Length;
        int lenBoth = lenDaily + lenConstant;

        this.items = new Item[lenBoth];

        // Store where the daily items get to
        int temp = 0;
        // Daily items
        for (var i = 0; i < lenDaily; i++)
        {
            this.items[i] = this.itemsDaily[i];
            temp = i;
        }
        // Constant items
        for (var j = 0; j < lenConstant; j++)
        {
            temp += 1;
            this.items[temp] = this.itemsConstant[j];
        }
    }

    /// <summary>
    /// Run the effects of the daily items
    /// </summary>
    public void RunEffects()
    {
        for (var i = 0; i < this.items.Length; i++)
        {
            // if the player has this item
            if (this.items[i].GetPlayerHas())
            {
                // if the item is a daily item
                if (this.items[i].GetItemType() == Item.Type.daily)
                {
                    this.items[i].ApplyItemEffect();
                }
            }
        }
    }

    /// <summary>
    /// Run the effects of the constant items
    /// </summary>
    public void RunEffects(Character charSelected, Decision decSelected, bool isApply)
    {
        for(var i = 0; i < this.items.Length; i++)
        {
            // if the player has this item
            if (this.items[i].GetPlayerHas())
            {
                // if the item is a constant item
                if (this.items[i].GetItemType() == Item.Type.constant)
                {
                    this.items[i].ApplyItemEffect(charSelected, decSelected, isApply);
                }
            }
        }
    }



    // UI FUNCTIONS

    /// <summary>
    /// Setup the UI of all the items
    /// </summary>
    /// <param name="prefab">The text box to instantiate and write to</param>
    /// <param name="parent">The parent object to set as the parent</param>
    /// <param name="position">The base position of the text box</param>
    public void SetupUI(TMP_Text prefab, GameObject parent, Vector2 position)
    {
        for (var i = 0; i < this.items.Length; i++)
        {
            this.items[i].SetupUI(prefab, parent, position);
        }
    }

    /// <summary>
    /// Update the UI of all the items
    /// </summary>
    public void UpdateUI()
    {
        for (var i = 0; i < this.items.Length; i++)
        {
            this.items[i].UpdateUI();
        }
    }
}
