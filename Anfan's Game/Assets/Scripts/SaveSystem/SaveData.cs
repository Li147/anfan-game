using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// stores all saved data
[Serializable]
public class SaveData
{
    public PlayerData MyPlayerData { get; set; }

    public List<ChestData> MyChestData { get; set; }

    public List<EquipmentData> MyEquipmentData { get; set; }

    public InventoryData MyInventoryData { get; set; }

    public List<ActionButtonData> MyActionBarData { get; set; }


    public SaveData()
    {
        MyInventoryData = new InventoryData();
        MyChestData = new List<ChestData>();
        MyEquipmentData = new List<EquipmentData>();
        MyActionBarData = new List<ActionButtonData>();
    }

}


[Serializable]
public class PlayerData
{
    public int MyLevel { get; set; }

    public float MyXp { get; set; }

    public float MyMaxXP { get; set; }

    public float MyHealth { get; set; }

    public float MyMaxHealth { get; set; }

    public float MyMana { get; set; }

    public float MyMaxMana { get; set; }

    public float MyHunger { get; set; }

    public float MyMaxHunger{ get; set; }

    public float MyX { get; set; }

    public float MyY { get; set; }

    public PlayerData(int level, float xp, float maxXp, float health, float maxHealth, float mana, float maxMana, 
                        float hunger, float maxHunger, Vector2 position)
    {
        this.MyLevel = level;
        this.MyXp = xp;
        this.MyMaxXP = maxXp;
        this.MyHealth = health;
        this.MyMaxHealth = maxHealth;
        this.MyMana = mana;
        this.MyMaxMana = maxMana;
        this.MyHunger = hunger;
        this.MyMaxHunger = maxHunger;
        this.MyY = position.y;
        this.MyX = position.x;

    }
}

[Serializable]
public class ItemData
{

    public string MyItemName { get; set; }
    public int MyStackCount { get; set; }
    public int MySlotIndex { get; set; }
    public int MyBagIndex { get; set; }

    public ItemData(string itemName, int stackCount = 0, int slotIndex = 0, int bagIndex = 0)
    {
        MyItemName = itemName;
        MyStackCount = stackCount;
        MySlotIndex = slotIndex;
        MyBagIndex = bagIndex;
    }

}

[Serializable]
public class ChestData
{
    public string MyName { get; set; }
    public List<ItemData> MyItems { get; set; }

    public ChestData(string name)
    {
        MyName = name;
        MyItems = new List<ItemData>();
    }
}

[Serializable]
public class InventoryData
{
    public List<BagData> MyBags { get; set; }

    public List<ItemData> MyItems { get; set; }

    public InventoryData()
    {
        MyBags = new List<BagData>();
        MyItems = new List<ItemData>();
    }
}



[Serializable]
public class BagData
{
    public int MySlotCount { get; set; }
    public int MyBagIndex { get; set; }

    public BagData(int count, int index)
    {
        MySlotCount = count;
        MyBagIndex = index;
    }

}

[Serializable]
public class EquipmentData
{
    public string MyName { get; set; }

    public string MyType { get; set; }

    public EquipmentData(string name, string type)
    {
        MyName = name;
        MyType = type;
    }
}

[Serializable]
public class ActionButtonData
{
    public string MyAction { get; set; }

    public bool IsItem { get; set; }

    public int MyIndex { get; set; }

    public ActionButtonData(string action, bool isItem, int index)
    {
        this.MyAction = action;
        this.IsItem = isItem;
        this.MyIndex = index;
    }

}

