using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IronOre", menuName = "Items/IronOre", order = 6)]
public class IronOre : Item
{

    public override string GetDescription()
    {

        return base.GetDescription() + string.Format("\nUseful crafting material.");

    }

}
