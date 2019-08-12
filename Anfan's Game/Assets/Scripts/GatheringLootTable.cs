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

    [SerializeField]
    private GameObject minimapIndicator;

    

    public bool isEmpty;


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
        Player.MyInstance.Gather(SpellBook.MyInstance.GetSpell("Gather"), this);
    
     
    }

    public void StopInteract()
    {
        if (isEmpty)
        {
            spriteRenderer.sprite = defaultSprite;
            gameObject.SetActive(false);
            minimapIndicator.SetActive(false);
        }
    }
}
