using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OpinionEffect
{
    [SerializeField] private int amount;

    public int GetOpinionAmount()
    {
        return this.amount;
    }
}
