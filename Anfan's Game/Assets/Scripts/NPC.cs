using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void HealthChanged(float health);

public delegate void CharacterRemoved();

public class NPC : Character
{

    public event HealthChanged healthChanged;

    public event CharacterRemoved characterRemoved;


    public virtual void DeSelect() {

        // Good practice to always include unsubscribe code in your thing

        healthChanged -= new HealthChanged(UIManager.MyInstance.UpdateTargetFrame);
        characterRemoved -= new CharacterRemoved(UIManager.MyInstance.HideTargetFrame);



    }

    public virtual Transform Select() {

        return hitBox;

    }

    public void OnHealthChanged(float health) {

        if (healthChanged != null) {

            healthChanged(health);

        }
        
    }

    public void OnCharacterRemoved() {

        if (characterRemoved != null) {
            characterRemoved();
        }
        Destroy(gameObject);

    }
}
