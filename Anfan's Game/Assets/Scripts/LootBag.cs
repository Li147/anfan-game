using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour, IInteractable
{
    private List<Item> contents;
    
    public void Initialize(List<Item> finalLoot)
    {
        this.contents = finalLoot;

    }

    public void Interact()
    {
        foreach (Item loot in contents)
        {
            ItemSpawnManager.MyInstance.SpawnEntities(loot.MyItemIndex, 1);
        }
        
        Destroy(this.gameObject);
    }

    public void StopInteract()
    {
        
    }
}
