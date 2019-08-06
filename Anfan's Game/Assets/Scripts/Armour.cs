using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ArmourType {Head, Chest, Legs, Feet, Accessory, MainHand, OffHand, TwoHand}

[CreateAssetMenu(fileName = "Armour", menuName = "Items/Armour", order = 2)]
public class Armour : Item
{
    [SerializeField]
    private ArmourType armourType;

    [SerializeField]
    private int strength;

    [SerializeField]
    private int defense;

    [SerializeField]
    private int stamina;

    [SerializeField]
    private AnimationClip[] animationClips;


    internal ArmourType MyArmourType { get => armourType;}
    public AnimationClip[] MyAnimationClips { get => animationClips;}

    public override string GetDescription() {

        string stats = string.Empty;

        if (strength > 0) {

            stats += string.Format("\n + {0} strength", strength);

        }

        if (defense > 0) {

            stats += string.Format("\n + {0} defense", defense);

        }

        if (stamina > 0) {

            stats += string.Format("\n + {0} stamina", stamina);

        }

        return base.GetDescription() + stats;
    }

    public void Equip() {

        CharacterPanel.MyInstance.EquipArmour(this);


    }



}
