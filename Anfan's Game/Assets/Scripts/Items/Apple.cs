using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Apple", menuName = "Items/Apple", order = 4)]
public class Apple : Item, IUseable
{
    [SerializeField]
    private int healthAmount;

    [SerializeField]
    private int hungerAmount;

    public void Use()
    {

        if (Player.MyInstance.MyHunger.MyCurrentValue < Player.MyInstance.MyHunger.MyMaxValue)
        {
            Remove();

            Player.MyInstance.GainHealth(healthAmount);
            Player.MyInstance.GainHunger(hungerAmount);

        }

    }


    public override string GetDescription()
    {

        return base.GetDescription() + string.Format("\nIt's an apple");

    }

}
