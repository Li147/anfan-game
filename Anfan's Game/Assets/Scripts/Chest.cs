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

    public List<Item> MyItems { get => items; set => items = value; }
    public BagScript MyBag { get => bag; set => bag = value; }

    private void Awake()
    {
        items = new List<Item>();
    }


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
            MyBag.Clear();
            isOpen = false;
            MyAnimator.SetBool("opened", false);
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
        }

    }

    // stores items in the list when chest is closed
    public void StoreItems()
    {
        MyItems = MyBag.GetItems();
    }

    // takes all stored items in the chest and displays in UI
    public void AddItems()
    {
        if (MyItems != null)
        {
            foreach (Item item in MyItems)
            {
                item.MySlot.AddItem(item);
            
            }
        }
    }

    

   
}
