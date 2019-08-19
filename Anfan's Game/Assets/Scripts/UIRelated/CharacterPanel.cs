using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : MonoBehaviour
{
    private static CharacterPanel instance;

    public static CharacterPanel MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<CharacterPanel>();
            }
            return instance;
        }
    }

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private CharButton main, off, head, chest, legs, feet, accessory1;

    private int baseDamage;
    private int baseDefence;
    private float baseSpeed;


    [SerializeField]
    private Text damageText;

    [SerializeField]
    private Text defenceText;

    [SerializeField]
    private Text speedText;





    public CharButton MySelectedButton { get; set; }

    public CharButton MyMain { get => main; set => main = value; }

    public void Start()
    {
        baseDamage = Player.MyInstance.MyBaseDamage;
        baseDefence = Player.MyInstance.MyBaseDefence;
        baseSpeed = Player.MyInstance.MyBaseMovementSpeed;
        UpdateStats();
    }


    public void EquipArmour(Armour armour) {

        switch (armour.MyArmourType)
        {
            case ArmourType.MainHand:
                MyMain.EquipArmour(armour);
                break;
            case ArmourType.OffHand:
                off.EquipArmour(armour);
                break;
            case ArmourType.Head:
                head.EquipArmour(armour);
                break;
            case ArmourType.Chest:
                chest.EquipArmour(armour);
                break;
            case ArmourType.Legs:
                legs.EquipArmour(armour);
                break;
            case ArmourType.Feet:
                feet.EquipArmour(armour);
                break;
            case ArmourType.Accessory:
               accessory1.EquipArmour(armour);
                break;
            
        }


        UpdateStats();
    }


    public void UpdateStats()
    {
        int bDamage = 0;
        int bDefence = 0;
        int bSpeed = 0;

        if (main.MyEquippedArmour != null)
        {
            bDamage += main.MyEquippedArmour.MyDamage;
            bDefence += main.MyEquippedArmour.MyDefence;
            bSpeed += main.MyEquippedArmour.MySpeed;

        }

        if (off.MyEquippedArmour != null)
        {
            bDamage += off.MyEquippedArmour.MyDamage;
            bDefence += off.MyEquippedArmour.MyDefence;
            bSpeed += off.MyEquippedArmour.MySpeed;

        }

        if (head.MyEquippedArmour != null)
        {
            bDamage += head.MyEquippedArmour.MyDamage;
            bDefence += head.MyEquippedArmour.MyDefence;
            bSpeed += head.MyEquippedArmour.MySpeed;
                
        }

        if (chest.MyEquippedArmour != null)
        {
            bDamage += chest.MyEquippedArmour.MyDamage;
            bDefence += chest.MyEquippedArmour.MyDefence;
            bSpeed += chest.MyEquippedArmour.MySpeed;

        }

        if (legs.MyEquippedArmour != null)
        {
            bDamage += legs.MyEquippedArmour.MyDamage;
            bDefence += legs.MyEquippedArmour.MyDefence;
            bSpeed += legs.MyEquippedArmour.MySpeed;

        }

        if (feet.MyEquippedArmour != null)
        {
            bDamage += feet.MyEquippedArmour.MyDamage;
            bDefence += feet.MyEquippedArmour.MyDefence;
            bSpeed += feet.MyEquippedArmour.MySpeed;

        }

        if (accessory1.MyEquippedArmour != null)
        {
            bDamage += accessory1.MyEquippedArmour.MyDamage;
            bDefence += accessory1.MyEquippedArmour.MyDefence;
            bSpeed += accessory1.MyEquippedArmour.MySpeed;

        }

        int totalDamage = bDamage + baseDamage;
        int totalDefence = bDefence + baseDefence;
        float totalSpeed = ((bSpeed / 100) * baseSpeed) + baseSpeed; 


        damageText.text = "Damage:\n" + totalDamage.ToString();
        defenceText.text = "Defence:\n" + totalDefence.ToString();
        speedText.text = "Speed:\n" + totalSpeed.ToString();

        Player.MyInstance.MyBonusDamage = bDamage;
        Player.MyInstance.MyBonusDefence = bDefence;
        Player.MyInstance.MyBonusSpeed = ((bSpeed / 100) * baseSpeed);


    }





}