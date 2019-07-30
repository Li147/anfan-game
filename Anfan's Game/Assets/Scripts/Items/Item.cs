using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A ScriptableObject is a script that exists in unity without an object
// Normally scripts are attatched to objects
// This means we can instantiate an object without it being physically in the game scene

public abstract class Item : ScriptableObject
{
    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private int stackSize;

    private SlotScript slot;


    public Sprite Icon { get => icon;}
    public int StackSize { get => stackSize;}
    protected SlotScript Slot { get => slot; set => slot = value; }
}
