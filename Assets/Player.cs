using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerClass
{
    Warrior,
    Assassin,
    Druid,
    Mage
}

public enum ClassType
{
    Physical,
    Magical
}

/*
 --== Physical ==--
Option 0 = reduce dual-wielding penalty to -25% dmg and -25% hit
Option 1 = increase damage by 25% if only using one weapon and no shield
Option 2 = increases your Critical strike chance by 5% and your chance to Hit by 10%.
Option 3 = Increases your chance to Dodge by 10% and your Resistance by 10%.
 --== Magical ==--
Option 4 = Add (100-(Dodge%))*0.16% to Dodge and (100-(Resistance%))*0.08 to Resistance.
Option 5 = Increases Intelligence by 15%. Adds 25% of Intelligence to Strength in damage calculations.
Option 6 = Every time your opponent resists one of your wand attacks they suffer their Resistance*2 in defendable damage.
Option 7 = Adds 25% of your Staff's average weapon damage to your armor & grants an additional attack with your staff every 3 rounds.
            Adds 10% of your Staff's average weapon damage to your Resistance. Adds 9% of your INT in damage to all Staff attacks.
*/
public enum T2Talent
{
    Option0,
    Option1,
    Option2,
    Option3,
    Option4,
    Option5,
    Option6,
    Option7
    
}

/*
 --== Physical ==--
Option 0 : Increases the chance to Block with a shield by 5% and inflicts your Speed plus 50% of your Strength damage on a successful Block.
Option 1 : Grants one additional melee attack every 2 Turns.
Option 2 : Increases Stamina by 50% and add 25% of Stamina to Strength in damage calculations.

 --== Magical ==--
Option 3 : Reduces your opponent’s Dodge by 10% and Resistance by 10%.
Option 4 : Regenerate 2.0% of your health at the start of your turn if you receive total damage equal or greater than 4.0% of your health on the previous turn.
Option 5 : Increases the basic attribute bonuses of your weapons by 100%, and your armor by 25%
*/
public enum T3Talent
{
    Option0,
    Option1,
    Option2,
    Option3,
    Option4,
    Option5
}

public class Player : MonoBehaviour
{
    [Header("Base")]
    public string username;

    // Health Points of the character
    // Maximum and current
    public int maxHP;
    public int currentHP;
    // The class the player has selected.
    public PlayerClass playerClass;
    // The level the player has reached.
    [Range(1, 40)]
    public int playerLevel = 1;


    [Space]


    [Header("Weapons")]
    // Each hand.
    public Weapon mainHand;
    public Weapon offHand;


    [Space]


    [Header ("BaseStats")]

    public int strength;

    public int speed, perception, intelligence, stamina, resist, armour;

    [Space]

    [Header("Talents")]
    public T1Talent tier1Talent;
    public T2Talent tier2Talent;
    public T3Talent tier3Talent;

    #region NonChanceEndStats

    [Space]

    [Header("End Stats - Not Chances")]

    public ClassType classType;
    // In a duel, highest initiative goes first.
    public int initiative;

    // % mitigation of physical damage.
    [Range(0, 90)]
	public int defense;

    // Multiplier for Physical Damage
    public float physicalMultiplier = 1;

    // Multiplier for Magical Damage
    public float magicalMultiplier = 1;

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
        // Set classType
        if (playerClass == PlayerClass.Assassin || playerClass == PlayerClass.Warrior)
            classType = ClassType.Physical;
        else if (playerClass == PlayerClass.Druid || playerClass == PlayerClass.Mage)
            classType = ClassType.Magical;
        else
            Debug.LogError("Invalid Class Type.");

        // Add weapon bonuses
        // mainHand
        strength += mainHand.strengthBonus;
        speed += mainHand.strengthBonus;
        perception += mainHand.perceptionBonus;
        intelligence += mainHand.intelligenceBonus;
        stamina += mainHand.staminaBonus;
        resist += mainHand.resistBonus;
        armour += mainHand.armourBonus;

        // offHand
        if (offHand != null)
        {
            strength += offHand.strengthBonus;
            speed += offHand.speedBonus;
            perception += offHand.perceptionBonus;
            intelligence += offHand.intelligenceBonus;
            stamina += offHand.staminaBonus;
            resist += offHand.resistBonus;
            armour += offHand.armourBonus;
        }

        // Calculate final stats
        // Base Stats
        maxHP = (int)((playerLevel * 50) + strength + (stamina * 4));

        // Non-Chance Stats
        initiative = (int)((perception + speed * 2) / 20);

        defense = (int)(0.154 * armour);

        physicalMultiplier = (float)((strength * 0.5) + (speed * 0.05) + (perception * 0.1) / 100);

        magicalMultiplier = (float)((intelligence * 0.5) + (speed * 0.05) + (perception * 0.1) / 100);

        // Chance Stats
        hitChance = (int)(70 + (((perception * 0.5) + speed) / 20));

        criticalStrikeChance = (int)(((perception * 1.5) + (speed * 0.5)) / 20);

        dodgeChance = (int)((intelligence + (perception * 0.5) + speed) / 20);

        //blockChance = shield;

        resistanceChance = (int)(0.234 * resist);

        // Final Stat prep
        currentHP = maxHP;

    }
}
