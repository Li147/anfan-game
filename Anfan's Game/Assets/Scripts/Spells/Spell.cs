﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Spell : IUseable, IMoveable, IDescribable
{
    [SerializeField]
    private string name;

    [SerializeField]
    private int damage;

    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float castTime;

    [SerializeField]
    private GameObject spellPrefab;

    [SerializeField]
    private string description;

    [SerializeField]
    private Color barColor;

    public string MyName { get => name; set => name = value; }
    public int MyDamage { get => damage; set => damage = value; }
    public Sprite MyIcon { get => icon; set => icon = value; }
    public float MySpeed { get => speed; set => speed = value; }
    public float MyCastTime { get => castTime; set => castTime = value; }
    public GameObject MySpellPrefab { get => spellPrefab; set => spellPrefab = value; }
    public Color MyBarColor { get => barColor; set => barColor = value; }

    public string GetDescription() {

        return string.Format("{0} \nCast time: {1} seconds\n{2}\nthat causes {3} damage.", name, castTime, description, damage);
    }

    public void Use() {

        Player.MyInstance.CastSpell(MyName);

    }
}
