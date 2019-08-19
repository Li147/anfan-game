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
    private int damage;

    [SerializeField]
    private int defence;

    [SerializeField]
    private int speed;

    [SerializeField]
    private AnimationClip[] animationClips;


    internal ArmourType MyArmourType { get => armourType;}
    public AnimationClip[] MyAnimationClips { get => animationClips;}
    public int MyDamage { get => damage; set => damage = value; }
    public int MyDefence { get => defence; set => defence = value; }
    public int MySpeed { get => speed; set => speed = value; }

    public override string GetDescription() {

        string stats = string.Empty;

        if (MyDamage > 0) {

            stats += string.Format("\n + {0} damage", MyDamage);

        }

        if (MyDefence > 0) {

            stats += string.Format("\n + {0} defence", MyDefence);

        }

        if (MySpeed > 0) {

            stats += string.Format("\n + {0} speed", MySpeed);

        }

        return base.GetDescription() + stats;
    }

    public void Equip() {

        CharacterPanel.MyInstance.EquipArmour(this);


    }



}
