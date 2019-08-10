using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTable : MonoBehaviour
{
    [SerializeField]
    protected Loot[] possibleLoot;

    public int[] quantity;

    public void Awake()
    {
        quantity = new int[possibleLoot.Length];
    }

    public void DropLoot(int itemIndex, int quantity) {

        ItemSpawnManager.MyInstance.SpawnEntities(itemIndex, quantity);

    }
    
    protected virtual void RollLoot() {

        foreach (Loot item in possibleLoot) {

            int roll = Random.Range(0, 100);

            if (roll <= item.MyDropChance) {

                DropLoot(item.MyItem.MyItemIndex, 1);

            }

        }

    }

    public void AccessRollLoot()
    {
        RollLoot();
    }


}
