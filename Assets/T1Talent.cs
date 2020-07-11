using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Stat Key:
0: Speed
1: Perception
2: Stamina
3: Strength
4: Armour
5: Resist
6: Intelligence
*/

public enum StatType
{
    Strength,
    Speed,
    Perception,
    Intelligence,
    Stamina,
    Resist,
    Armour
}

[CreateAssetMenu (fileName="Tier 1 Talent", menuName="Character Attributes/Tier 1 Talent")]
public class T1Talent : ScriptableObject
{
    [Header("Base Info")]
    public string talentID;
    public string talentName;
    [TextArea]
    public string talentDescription;


    public PlayerClass skillClass;

    [Space]

    [Header("Stats")]
    public int primaryStat;
    public StatType primaryStatType;
    public int secodaryStat = 0;
    public StatType scondaryStatType;

    [Space]

    [Header("Condition(s)")]
    public WeaponType weaponType;

    public bool ClassCheck(Player player)
    {
        if (player.playerClass != skillClass)
            return false;

        return true;
    }

    public bool GetsBenefit(WeaponType theWeaponsType)
    {
        if (theWeaponsType == weaponType)
            return true;
        else 
            return false;

    }
}