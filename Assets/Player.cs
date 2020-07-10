using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Key:
0 = Physical
1 = Magical
*/
public enum AttackTypes : int
{
    Physical,
    Magical
};

public class Player : MonoBehaviour
{
    [Header("Base")]
    public string username;

    // Health Points of the character
    // Maximum and current
    public int maxHP;
    public int currentHP;

    [Space]

    [Header("Weapons")]
    public AttackTypes attackType;
    // Min and Max for Weapon Damage
    public int minWeaponDamage;
    public int maxWeaponDamage;
    // Chance to hit the target
    [Range(0, 100)]
    public int hitChance;
    // Are they duel wielding?
    public bool isDuelWield;

    [Space]

    [Header("Damage Increasers")]
    // Multiplier for Physical Damage
    public float physicalMultiplier = 1;
    // Multiplier for Magical Damage
    public float magicalMultiplier = 1;
    // Chance to hit a critical strike;
    [Range(0, 100)]
    public int criticalStrikeChance;

    [Space]

    [Header("Damage Reducers")]
    // % mitigation of physical damage.
    [Range(0, 100)]
	public int defense;
    // Chance to avoid all physical damage (one hit)
    [Range(0, 100)]
    public int dodgeChance;
    // Chance to block all incoming physical damage
    [Range(0, 100)]
    public int blockChance;
    // Chance to avoid all magical damage (one hit)
    [Range(0, 100)]
    public int resistanceChance;

    [Space]

    [Header("Misc")]
    // In a duel, highest initiative goes first.
    public int initiative;
}
