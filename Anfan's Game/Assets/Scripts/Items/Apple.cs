using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Apple", menuName = "Items/Apple", order = 4)]
public class Apple : Item
{

    public override string GetDescription()
    {

        return base.GetDescription() + string.Format("\nIt's an apple");

    }

}
