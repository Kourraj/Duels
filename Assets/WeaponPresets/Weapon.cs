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
public enum WeaponType
{
    Dagger = 0,
    Sword = 1,
    Hammer = 2,
    Axe = 3,
    Staff = 4,
    Rod = 5,
    Wand = 6
}

public enum WeaponRarity
{
    Green,
    Blue,
    Purple,
    Orange
}

[CreateAssetMenu (fileName="weapon", menuName="Items/Weapon")]
public class Weapon : ScriptableObject
{
    [Header("Base Info")]
    public string itemID;
    public string weaponName;
    [TextArea]
    public string weaponDescription;

    public WeaponType weaponType;
    public int minDamage, maxDamage;

    [Space]

    [Header("Bonus Stats")]
    [Tooltip("Amount depends on rarity")]
    public int strengthBonus;
    [Tooltip("Amount depends on rarity")]
    public int speedBonus, perceptionBonus, intelligenceBonus, staminaBonus, resistBonus, armourBonus;
}
