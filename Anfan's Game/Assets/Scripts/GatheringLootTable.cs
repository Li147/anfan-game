using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatheringLootTable : LootTable, IInteractable
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Sprite defaultSprite;

    [SerializeField]
    private Sprite gatherSprite;

    private bool isEmpty;


    private void Start()
    {
        RollLoot();
    }


    protected override void RollLoot()
    {
        for (int i = 0; i < possibleLoot.Length;  i++)
        {
            int roll = Random.Range(1, 6);
            quantity[i] = roll;
        }
    }

    public void Interact()
    {
        Player.MyInstance.Gather("Gather", possibleLoot, quantity);
        isEmpty = true;
     
    }

    public void StopInteract()
    {
        if (isEmpty)
        {
            spriteRenderer.sprite = defaultSprite;
            gameObject.SetActive(false);
        }
    }
}
