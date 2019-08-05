using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTable : MonoBehaviour
{
    [SerializeField]
    private Loot[] possibleLoot;

    private List<Item> droppedItems = new List<Item>();

    public void DropLoot(int itemIndex) {

        SpawnItemController.MyInstance.SpawnEntities(itemIndex, 1);

    }
    
    public void RollLoot() {

        foreach (Loot item in possibleLoot) {

            int roll = Random.Range(0, 100);

            if (roll <= item.MyDropChance) {

                DropLoot(item.MyItem.MyItemIndex);

            }

        }

    }


}
