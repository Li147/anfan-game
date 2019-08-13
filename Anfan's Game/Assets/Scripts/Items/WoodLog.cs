using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WoodLog", menuName = "Items/WoodLog", order = 7)]
public class WoodLog : Item
{

    public override string GetDescription()
    {

        return base.GetDescription() + string.Format("\nAn important material for many crafting recipes.");

    }

}
