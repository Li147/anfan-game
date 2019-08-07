using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class NPC : IInteractable
{
    public virtual void Interact()
    {
        // need to implement later on
        Debug.Log("This will open dialogue with NPC");

    }


    public virtual void StopInteract() {
        throw new System.NotImplementedException();
    }
}
