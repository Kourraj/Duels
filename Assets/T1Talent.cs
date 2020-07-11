using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    CriticalChance,
    Damage,
    HitChance
}

[CreateAssetMenu (fileName="Tier 1 Talent", menuName="Character Attributes/Tier 1 Talent")]
public class T1Talent : ScriptableObject
{
    [Header("Base Info")]
    public string talentID;
    public string talentName;
    [TextArea]
    public string talentDescription;

    [Space]

    [Header("Stats")]
    public int primaryStat;
    public StatType primaryStatType;
    public int secodaryStat = 0;
    public StatType scondaryStatType;

    [Space]

    [Header("Condition(s)")]
    public ClassType classType;
    public WeaponType weaponType;

    public bool ClassCheck(ClassType playerClassType)
    {
        if (playerClassType == classType)
            return true;

        return false;
    }
    
    public bool GetsBenefit(WeaponType playerWeaponType)
    {
        if (weaponType == playerWeaponType)
            return true;
        return false;
    }
    
}