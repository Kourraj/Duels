using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Key:
0 = Physical
1 = Magical
*/
public enum AttackTypes
{
    Physical,
    Magical
};

public enum PlayerClass
{
    Warrior,
    Assassin,
    Druid,
    Mage
}

public class Player : MonoBehaviour
{
    [Header("Base")]
    public string username;

    // Health Points of the character
    // Maximum and current
    public int maxHP;
    public int currentHP;
    public PlayerClass playerClass;


    [Space]


    [Header("Weapons")]
    // Each hand.
    public Weapon mainHand;
    public Weapon offHand;


    [Space]


    [Header ("BaseStats")]

    public int strength;

    public int speed, perception, intelligence, stamina, resist, armour;

    #region NonChanceEndStats
    [Space]


    [Header("End Stats - Not Chances")]

    // Multiplier for Physical Damage
    public float physicalMultiplier = 1;

    // Multiplier for Magical Damage
    public float magicalMultiplier = 1;

    // In a duel, highest initiative goes first.
    public int initiative;
    #endregion NonChanceEndStats

    #region ChanceEndStats
    [Space]

    [Header("End Stats - Chances")]
    // Chance to hit a target
    [Range(0, 100)]
    public int hitChance;

    // Chance to hit a critical strike;
    [Range(0, 90)]
    public int criticalStrikeChance;

    // % mitigation of physical damage.
    [Range(0, 90)]
	public int defense;

    // Chance to avoid all physical damage (one hit)
    [Range(0, 90)]
    public int dodgeChance;

    // Chance to block all incoming physical damage
    [Range(0, 90)]
    public int blockChance;

    // Chance to avoid all magical damage (one hit)
    [Range(0, 90)]
    public int resistanceChance;
    #endregion ChanceEndStats
    
    void Awake ()
    {
        defense = (int)(0.184 * armour);
        physicalMultiplier = (int)((strength * 0.5) + (speed * 0.05) + (perception * 0.1));
    }
}
