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

    public void Interact()
    {
        if (isOpen)
        {
            StopInteract();
            

        }
        else
        {
            
            MyAnimator.SetBool("opened", true);
            isOpen = true;

        }
    }

    public void StopInteract()
    {
        isOpen = false;
        MyAnimator.SetBool("opened", false);
    }

    

   
}
