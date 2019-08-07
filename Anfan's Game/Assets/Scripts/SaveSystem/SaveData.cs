using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// stores all saved data
[Serializable]
public class SaveData
{
    public PlayerData MyPlayerData { get; set; }

    public SaveData()
    {

    }

}


[Serializable]
public class PlayerData
{
    public int MyLevel { get; set; }

    public PlayerData(int level)
    {
        this.MyLevel = level;
    }
}
