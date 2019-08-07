using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class EXPManager
{
    public static int CalculateXP(Enemy e)
    {
        // XP = (char lvl * 5) + 45, where char level = mob level
        int baseXP = (Player.MyInstance.MyLevel * 5) + 45;

        int grayLevel = CalculateGrayLevel();

        int totalXP = 0;
        // our formula: XP = (base xp)  * (1 + 0.05 * (mob lvl - char lvl))  where mob > player lvl

        if (e.MyLevel >= Player.MyInstance.MyLevel)
        {
            totalXP= (int) (baseXP * (1 + 0.05 * (e.MyLevel - Player.MyInstance.MyLevel)));
        }
        else if ( e.MyLevel > grayLevel)
        {
            totalXP = (baseXP) * (1 - (Player.MyInstance.MyLevel - e.MyLevel) / ZeroDifference());
        }

        return totalXP;
    }

    private static int ZeroDifference()
    {
        int lvl = Player.MyInstance.MyLevel;

        if (lvl <= 7)
        {
            return 5;
        }
        if (lvl >= 8 && lvl <= 9)
        {
            return 6;
        }
        if (lvl >= 10 && lvl <= 11)
        {
            return 7;
        }
        if (lvl >= 12 && lvl <= 15)
        {
            return 8;
        }
        if (lvl >= 16 && lvl <= 19)
        {
            return 9;
        }
        if (lvl >= 20 && lvl <= 29)
        {
            return 11;
        }
        if(lvl >= 30 && lvl <= 39)
        {
            return 12;
        }
        if (lvl >= 40 && lvl <= 44)
        {
            return 13;
        }
        if (lvl >= 45 && lvl <= 49)
        {
            return 14;
        }
        if (lvl >= 50 && lvl <= 54)
        {
            return 15;
        }
        if (lvl >= 55 && lvl <= 59)
        {
            return 16;
        }
        return 17;
    }

    public static int CalculateGrayLevel()
    {
        int lvl = Player.MyInstance.MyLevel;

        if (lvl <= 5)
        {
            return 0;
        }
        else if (lvl >= 6 && lvl <= 49)
        {
            return lvl - (lvl / 10) - 5;
        }
        else if (lvl == 50)
        {
            return lvl - 10;
        }
        else if (lvl >= 51 && lvl <= 59)
        {
            return lvl - (lvl / 5) - 1;
        }

        return lvl - 9;
    }

}
