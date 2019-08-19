using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTable : MonoBehaviour
{
    [SerializeField]
    public Loot[] possibleLoot;

    [SerializeField]
    LootBag lootBagPrefab;

    public int[] quantity;

    public void Awake()
    {
        quantity = new int[possibleLoot.Length];
    }

    public void SpawnLootBag()
    {
        LootBag loot = Instantiate(lootBagPrefab, transform.position, Quaternion.identity);
        loot.Initialize(RollLoot());
    }

    
    protected virtual List<Item> RollLoot()
    {
        List<Item> finalLoot = new List<Item>();

        foreach (Loot loot in possibleLoot) {

            int roll = Random.Range(0, 100);

            if (roll <= loot.MyDropChance) {

                finalLoot.Add(loot.MyItem);

            }

        }

        return finalLoot;

    }



}
