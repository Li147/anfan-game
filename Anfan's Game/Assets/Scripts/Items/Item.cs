using System.Collections;
using System.Collections.Generic;
using UnityEngine;




// A ScriptableObject is a script that exists in unity without an object
// Normally scripts are attatched to objects
// This means we can instantiate an object without it being physically in the game scene

public abstract class Item : ScriptableObject, IMoveable, IDescribable
{
    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private int stackSize;

    [SerializeField]
    private string itemName;

    [SerializeField]
    private int itemIndex;

    [SerializeField]
    private Quality quality;

    private SlotScript slot;

    public CharButton MyCharButton { get; set; }


    public Sprite MyIcon { get => icon;}
    public int MyStackSize { get => stackSize;}
    public SlotScript MySlot { get => slot; set => slot = value; }
    public int MyItemIndex { get => itemIndex;}
    public string MyName { get => itemName; set => itemName = value; }

    public virtual string GetDescription() {

        return string.Format("<color={0}> {1}</color>", QualityColor.MyColors[quality], MyName);
    
    }

    public void Remove() {

        if (MySlot != null) {
            MySlot.RemoveItem(this);
            MySlot = null;
        }

    }


}
