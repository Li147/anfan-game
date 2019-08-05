using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPanel : MonoBehaviour
{

    private static CharacterPanel instance;



    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private CharButton head, chest, legs, accessory1, accessory2, accessory3, main, off;

    public CharButton MySelectedButton { get; set; }

    public static CharacterPanel MyInstance {

        get {
            if (instance == null) {
                instance = GameObject.FindObjectOfType<CharacterPanel>();
            }

            return instance;
        }
            
    }

    public void OpenClose() {

        if (canvasGroup.alpha <= 0) {

            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1;

        } else {

            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0;

        }

    }

    public void EquipArmour(Armour armour) {

        switch (armour.MyArmourType) {
            case ArmourType.Head:

                head.EquipArmour(armour);

                break;
            case ArmourType.Chest:
                chest.EquipArmour(armour);
                break;
            case ArmourType.Legs:
                legs.EquipArmour(armour);
                break;
            case ArmourType.Accessory:
               accessory1.EquipArmour(armour);
                break;
            case ArmourType.MainHand:
                main.EquipArmour(armour);
                break;
            case ArmourType.OffHand:
                off.EquipArmour(armour);
                break;
            
        }

    }
}