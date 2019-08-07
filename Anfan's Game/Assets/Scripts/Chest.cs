using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// script for the chest item
public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Animator MyAnimator;

    private bool isOpen;

    [SerializeField]
    private CanvasGroup canvasGroup;

    // list of items that belong to this specific chest
    private List<Item> items;

    [SerializeField]
    private BagScript bag;

    // When you left click on the chest, GameManager class will call this function
    public void Interact()
    {
        if (isOpen)
        {
            StopInteract();
            

        }
        else
        {
            AddItems();
            MyAnimator.SetBool("opened", true);
            isOpen = true;
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;

        }
    }

    public void StopInteract()
    {
        if (isOpen)
        {
            StoreItems();
            bag.Clear();
            isOpen = false;
            MyAnimator.SetBool("opened", false);
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
        }

    }

    // stores items in the list when chest is closed
    public void StoreItems()
    {
        items = bag.GetItems();
    }

    // takes all stored items in the chest and displays in UI
    public void AddItems()
    {
        if (items != null)
        {
            foreach (Item item in items)
            {
                item.MySlot.AddItem(item);
            
            }
        }
    }

    

   
}
