using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string username;

    // Health Points of the character
    // Maximum and current
    public int maxHP;
    public int currentHP;

    // Min and Max for Weapon Damage
    public int minWeaponDamage;
    public int maxWeaponDamage;
    // Chance to hit the target
    public int hitChance;
    // Are they duel wielding?
    public bool isDuelWield;

    // Multiplier for Physical Damage
    public float physicalMultiplier = 1;
    // Multiplier for Magical Damage
    public float magicalMultiplier = 1;
    // Chance to hit a critical strike;
    public int criticalStrikeChance;

    // % mitigation of physical damage.
	public int defense;
    // Chance to avoid all physical damage (one hit)
    public int dodgeChance;
    // Chance to block all incoming physical damage
    public int blockChance;
    // Chance to avoid all magical damage (one hit)
    public int resistanceChance;

    // In a duel, highest initiative goes first.
    public int initiative;
}
