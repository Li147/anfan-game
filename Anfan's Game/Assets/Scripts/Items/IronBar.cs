using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IronBar", menuName = "Items/IronBar", order = 5)]
public class IronBar : Item
{

    public override string GetDescription()
    {

        return base.GetDescription() + string.Format("\nIt's an iron bar.");

    }

}
