using System;
using UnityEngine;

public class Duel : MonoBehaviour
{
    public Player attacker;
    public Player defender;

    System.Random random;

    int duelDamageMult = 1;

    public int randomSeed = 5;
    public void HaveDuel()
    {
        random = new System.Random(randomSeed);
        Player first;
        Player second;
        if (attacker.initiative < defender.initiative)
        {
            first = defender;
            second = attacker;
        }
        else if (defender.initiative < attacker.initiative)
        {
            first = attacker;
            second = defender;
        }
        else
        {
            if (random.Next(1) == 1)
            {
                first = attacker;
                second = defender;
            }
            else
            {
                first = defender;
                second = attacker;
            }
        }

        // Stops ∞ turns as they don't deal damage.
        int turnCount = 0;
        Player victor = null;
        while (true)
        {
            // Turn Count.
            turnCount ++;
            // Caps turn count, if no damage happens, the duel won't go on forever.
            if (turnCount >= 100)
                break;
            // Display the turn
            Debug.Log("Turn " + turnCount);

            // Actions
            if (DoAction(first, second))
                break;
            if (DoAction(second, first))
                break;

            // Skills
            if (DoSkill(first, second))
                break;
            if (DoSkill(second, first))
                break;

            // Do Attacks
            victor = DoAttacks(first, second);
            if (victor != null)
                break;

            // Bleed damage.
            if (turnCount > 15)
            {
                if (turnCount % 2 == 1)
                {
                    int turnDmg = 50 + ((turnCount - 16) * 10);
                    int bleedDmg = random.Next(turnDmg - (turnDmg / 10), turnDmg + (turnDmg / 10));
                    Debug.Log(second.username + " bleeds for " + bleedDmg + " damage.");
                    Debug.Log(second.username + " is on " + second.currentHP + "❤");
                }
                else if (turnCount % 2 == 0)
                {
                    int turnDmg = 50 + ((turnCount - 16) * 10);
                    int bleedDmg = random.Next(turnDmg - (turnDmg / 10), turnDmg + (turnDmg / 10));
                    Debug.Log(first.username + " bleeds for " + bleedDmg + " damage.");
                    Debug.Log(first.username + " is on " + first.currentHP + "❤");
                }
            }
        }

        // Victory stuff
        Debug.Log("And the winner is.... " + victor.username);
        Debug.Log(turnCount);
    }

    int CalculateAttacks(Player attacker)
    {
        // Calculate moves.
        int attackCount = 1;
        if (attacker.offHand != null)
            attackCount += 1;
        // TODO - Insert more check for extra attacks

        return attackCount;
    }

    Player DoAction(Player first, Player second)
    {
        return null;
    }
    Player DoSkill(Player first, Player second)
    {
        return null;
    }

    /* IGNORE. Code to trial Talent implementation
    Player DoNewMHAttack(Player attacker, Player defender)
    {
        // Check if they hit the person.
        int baseHitChance = attacker.hitChance;
        string[] hitChanceTalents
        if (attacker.tier1Talent.id  "PT1SD")
        return null;
    }
    */

    Player DoMainHandAttack(Player attacker, Player defender)
    {
        // attacker Attacks

        // Check they hit the person.
        if (random.Next(1, 100) > attacker.hitChance)
        {
            Debug.Log(attacker.username + " took a swing at " + defender.username + " and missed!");
        }
        else if (random.Next(1, 100) <= defender.blockChance)
        {
            Debug.Log(defender.username + " blocked a hit from " + attacker.username + "!");
        }
        else if (random.Next(1, 100) <= defender.dodgeChance)
        {
            Debug.Log(defender.username + " dodged an attack from " + attacker.username + "!");
        }
        // They hit!
        else
        {
            // Calculate the damage dealt.
            int weapDam = random.Next(attacker.mainHand.minDamage, attacker.mainHand.maxDamage);
            int damage = (int)Math.Floor((weapDam * attacker.physicalMultiplier) * ((float)(100 - defender.defense) / 100));
            damage *= duelDamageMult;
            if (random.Next(1, 100) < attacker.criticalStrikeChance)
            {
                if (random.Next(1, 100) < 20)
                {
                    // SUPER CRITICAL!
                    // Double original damage and ignores defence.
                    damage = (int)Math.Floor((damage / ((float)(100 - defender.defense) / 100)) * 2);
                    Debug.Log(attacker.username + " smashed " + defender.username + " for a SUPER critical hit! They dealt " + damage.ToString() + " damage!");
                    defender.currentHP -= damage;
                    Debug.Log(defender.username + " is on " + defender.currentHP + "❤");
                }
                else
                {
                    // Critical Hit!
                    damage = (int)Math.Floor((damage / ((float)(100 - defender.defense) / 100)) * 1.5);
                    Debug.Log(attacker.username + " smacked " + defender.username + " for a critical hit! They dealt " + damage.ToString() + " damage!");
                    defender.currentHP -= damage;
                    Debug.Log(defender.username + " is on " + defender.currentHP + "❤");
                }
            }
            else
            {
                // Just a normal hit.
                Debug.Log(attacker.username + " hit " + defender.username + " for " + damage.ToString() + " damage!");
                defender.currentHP -= damage;
                Debug.Log(defender.username + " is on " + defender.currentHP + "❤");
            }

            // Did they die?
            if (defender.currentHP <= 0)
                return attacker;
        }

        return null;
    }

    Player DoOffHandAttack (Player attacker, Player defender)
    {
        // attacker Attacks

        // Check they hit the person.
        
        if (random.Next(1, 100) > (attacker.hitChance * .5))
        {
            Debug.Log(attacker.username + " took a swing at " + defender.username + " and missed!");
        }
        else if (random.Next(1, 100) <= defender.blockChance)
        {
            Debug.Log(defender.username + " blocked a hit from " + attacker.username + "!");
        }
        else if (random.Next(1, 100) <= defender.dodgeChance)
        {
            Debug.Log(defender.username + " dodged an attack from " + attacker.username + "!");
        }
        // They hit!
        else
        {
            // Calculate the damage dealt.
            int weapDam = random.Next(attacker.offHand.minDamage, attacker.offHand.maxDamage);
            int damage = (int)Math.Floor((weapDam * attacker.physicalMultiplier) * ((float)(100 - defender.defense) / 100));
            damage = (int)Math.Floor(damage * 0.5);
            damage *= duelDamageMult;
            if (random.Next(1, 100) < attacker.criticalStrikeChance)
            {
                if (random.Next(1, 100) < 20)
                {
                    // SUPER CRITICAL!
                    // Double original damage and ignores defence.
                    damage = (int)Math.Floor((damage / ((float)(100 - defender.defense) / 100)) * 2);
                    Debug.Log(attacker.username + " smashed " + defender.username + " with their secondary weapon for a SUPER critical hit! They dealt " + damage.ToString() + " damage!");
                    defender.currentHP -= damage;
                    Debug.Log(defender.username + " is on " + defender.currentHP + "❤");
                }
                else
                {
                    // Critical Hit!
                    damage = (int)Math.Floor((damage / ((float)(100 - defender.defense) / 100)) * 1.5);
                    Debug.Log(attacker.username + " smacked " + defender.username + " with their secondary weapon for a critical hit! They dealt " + damage.ToString() + " damage!");
                    defender.currentHP -= damage;
                    Debug.Log(defender.username + " is on " + defender.currentHP + "❤");
                }
            }
            else
            {
                // Just a normal hit.
                Debug.Log(attacker.username + " hit " + defender.username + " with their secondary weapon for " + damage.ToString() + " damage!");
                defender.currentHP -= damage;
                Debug.Log(defender.username + " is on " + defender.currentHP + "❤");
            }

            // Did they die?
            if (defender.currentHP <= 0)
                return attacker;
        }

        return null;
    }

    Player DoOffHandWandAttack(Player attacker, Player defender)
    {
        // attacker Attacks

        // Check they hit the person.
        if (random.Next(1, 100) > attacker.hitChance)
        {
            Debug.Log(attacker.username + " took a swing at " + defender.username + " and missed!");
        }
        // They hit!
        else
        {
            // Calculate the damage dealt.
            int weapDam = random.Next(attacker.offHand.minDamage, attacker.offHand.maxDamage);
            int damage = (int)Math.Floor((weapDam * attacker.magicalMultiplier));
            damage *= duelDamageMult;
            if (random.Next(1, 100) < attacker.criticalStrikeChance)
            {
                if (random.Next(1, 100) < 20)
                {
                    // SUPER CRITICAL!
                    // Double original damage and ignores defence.
                    damage = damage * 2;
                    Debug.Log(attacker.username + " magicked " + defender.username + " for a SUPER critical hit! They dealt " + damage.ToString() + " damage!");
                    defender.currentHP -= damage;
                    Debug.Log(defender.username + " is on " + defender.currentHP + "❤");
                }
                else
                {
                    // Critical Hit!
                    damage = (int)Math.Floor(damage * 1.5);
                    Debug.Log(attacker.username + " magicked " + defender.username + " for a critical hit! They dealt " + damage.ToString() + " damage!");
                    defender.currentHP -= damage;
                    Debug.Log(defender.username + " is on " + defender.currentHP + "❤");
                }
            }
            else
            {
                // Check if it was resisted
                if (random.Next(1, 100) <= defender.resistanceChance)
                {
                    Debug.Log(defender.username + " resisted an attack from " + attacker.username + "!");
                }
                // Just a normal hit.
                else
                {
                    Debug.Log(attacker.username + " magicked " + defender.username + " for " + damage.ToString() + " damage!");
                    defender.currentHP -= damage;
                    Debug.Log(defender.username + " is on " + defender.currentHP + "❤");
                }
            }

            // Did they die?
            if (defender.currentHP <= 0)
                return attacker;
        }

        return null;
    }

    Player DoAttacks(Player first, Player second)
    {
        // Calculate attacks
        int firstAttackCount = CalculateAttacks(first);
        int secondAttackCount = CalculateAttacks(second);
        int firstAttacksMade = 0;
        int secondAttacksMade = 0;
        // Attacks

        while (firstAttackCount != firstAttacksMade && secondAttackCount != secondAttacksMade)
        {
            // First's turn to attack.
            if (firstAttacksMade == secondAttacksMade || secondAttackCount == secondAttacksMade)
            {
                if ((firstAttacksMade % 2) == 0 || first.offHand == null)
                    if (DoMainHandAttack(first, second))
                        return first;
                else
                    if (!(first.offHand.weaponType == WeaponType.Wand))
                        if (DoOffHandAttack(first, second))
                            return first;
                    // Wand offhand
                    if (first.offHand.weaponType == WeaponType.Wand)
                        if (DoOffHandWandAttack(first, second))
                            return first;

                firstAttacksMade += 1;
            }
            else
            {
                if ((secondAttacksMade % 2) == 0 || second.offHand == null)
                    if (DoMainHandAttack(second, first))
                        return second;
                else
                    if (!(second.offHand.weaponType == WeaponType.Wand))
                        if (DoOffHandAttack(second, first))
                            return second;
                    // Wand offhand
                    if (second.offHand.weaponType == WeaponType.Wand)
                        if (DoOffHandWandAttack(second, first))
                            return second;

                secondAttacksMade += 1;
            }

        }
        return null;
    }
}
