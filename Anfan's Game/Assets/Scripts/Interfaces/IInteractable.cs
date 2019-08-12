using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An interface that is implemented on all things we can "interact" with e.g. enemy loot, chests, trees
public interface IInteractable
{
    void Interact();

    void StopInteract();
}
