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
    private Sprite emptySprite;

    [SerializeField]
    private GameObject minimapIndicator;

    [SerializeField]
    private bool isDestroyable;

    

    public bool isEmpty;


    private void Start()
    {
        RollLoot();
    }

    private void Update()
    {
        if (isEmpty && isDestroyable)
        {
            Destroy(transform.parent.gameObject);
        }
        else if (isEmpty)
        {
            spriteRenderer.sprite = emptySprite;
            gameObject.SetActive(false);
            minimapIndicator.SetActive(false);
        }
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
        if (isEmpty && isDestroyable)
        {
            Destroy(transform.parent.gameObject);
        }
        else if (isEmpty)
        {
            spriteRenderer.sprite = emptySprite;
            gameObject.SetActive(false);
            minimapIndicator.SetActive(false);
        }
    }
}
