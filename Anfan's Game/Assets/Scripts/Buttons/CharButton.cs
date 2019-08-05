using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler 
{
    [SerializeField]
    private ArmourType armorType;

    private Armour equippedArmour;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private GearSocket gearSocket;

    public void OnPointerClick(PointerEventData eventData) {
        
        if (eventData.button == PointerEventData.InputButton.Left) {

            if (HandScript.MyInstance.MyMoveable is Armour) {

                Armour tmp = (Armour) HandScript.MyInstance.MyMoveable;

                if (tmp.MyArmourType == armorType) {

                    EquipArmour(tmp);

                }
                UIManager.MyInstance.RefreshTooltip(tmp);
            }
            // check if your hand is empty and we are clicking on a charbutton with armor equipped
            else if(HandScript.MyInstance.MyMoveable == null && equippedArmour != null) {

                HandScript.MyInstance.TakeMoveable(equippedArmour);
                CharacterPanel.MyInstance.MySelectedButton = this;
                icon.color = Color.gray;

            }


        }

    }

    public void EquipArmour(Armour armour) {

        armour.Remove();

        if (equippedArmour != null) {

            if (equippedArmour != armour) {

                armour.MySlot.AddItem(equippedArmour);

            }
            
            UIManager.MyInstance.RefreshTooltip(equippedArmour);



        } else {

            UIManager.MyInstance.HideTooltip();

        }

        icon.enabled = true;
        icon.sprite = armour.MyIcon;
        icon.color = Color.white;
        this.equippedArmour = armour;  // references the equipped armour

        if (HandScript.MyInstance.MyMoveable == (armour as IMoveable)) {

            HandScript.MyInstance.Drop();
             
        }

        // check that the charbutton has an associated socket and that armor has animation clips
        if (gearSocket != null && equippedArmour.MyAnimationClips != null) {

            gearSocket.EquipAnimation(equippedArmour.MyAnimationClips);

        }
        

    }

    public void OnPointerEnter(PointerEventData eventData) {

        if (equippedArmour != null) {

            UIManager.MyInstance.ShowTooltip(new Vector2(0,0), transform.position, equippedArmour);

        }
    }

    public void OnPointerExit(PointerEventData eventData) {

        UIManager.MyInstance.HideTooltip();
        
    }

    public void DequipArmour() {

        icon.color = Color.white;
        icon.enabled = false;
        

        if (gearSocket != null && equippedArmour.MyAnimationClips != null) {

            gearSocket.DequipAnimation();

        }

        equippedArmour = null;
            
    }
}
