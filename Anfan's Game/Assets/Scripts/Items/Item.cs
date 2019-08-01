using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A ScriptableObject is a script that exists in unity without an object
// Normally scripts are attatched to objects
// This means we can instantiate an object without it being physically in the game scene

public abstract class Item : ScriptableObject, IMoveable
{
    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private int stackSize;

    private SlotScript slot;


    public Sprite MyIcon { get => icon;}
    public int MyStackSize { get => stackSize;}
    public SlotScript MySlot { get => slot; set => slot = value; }

    public void Remove() {

        if (MySlot != null) {
            MySlot.RemoveItem(this);
        }

    }


}
