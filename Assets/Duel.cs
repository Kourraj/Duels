using System;
using UnityEngine;

public class Duel : MonoBehaviour
{
    public Player attacker;
    public Player defender;

    private Player victor;

    System.Random random;

    void Start()
    {
        haveDuel();
    }
    public int randomSeed = 5;
    void haveDuel()
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
        while (true)
        {
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

            // Attacks
            // Physical Attacks
            if (DoPhysicalAttack(first, second))
                break;
            if (first.isDuelWield)
                if (DoSecondaryPhysicalAttack(first, second))
                    break;
            if (DoPhysicalAttack(second, first))
                break;
            if (second.isDuelWield)
                if (DoSecondaryPhysicalAttack(first, second))
                    break;

            turnCount ++;
            if (turnCount >= 70)
                break;
        }

        // Victory stuff
        Debug.Log("And the winner is.... " + victor.username);
        Debug.Log(turnCount);
    }

    bool DoAction(Player first, Player second)
    {
        return false;
    }
    bool DoSkill(Player first, Player second)
    {
        return false;
    }
    bool DoPhysicalAttack(Player attacker, Player defender)
    {
        // attacker Attacks

        // Check they hit the person.
        if (random.Next(1, 100) > attacker.hitChance)
        {
            Debug.Log(attacker.username + " took a swing at " + defender.username + " and missed!");
        }
        else if (random.Next(1, 100) <= defender.dodgeChance)
        {
            Debug.Log(defender.username + " dodged an attack from " + attacker.username + "!");
        }
        else if (random.Next(1, 100) <= defender.blockChance)
        {
            Debug.Log(defender.username + " blocked a hit from " + attacker.username + "!");
        }
        // They hit!
        else
        {
            // Calculate the damage dealt.
            int weapDam = random.Next(attacker.minWeaponDamage, attacker.maxWeaponDamage);
            int damage = (int)Math.Floor((weapDam * attacker.physicalMultiplier) * ((float)(100 - defender.defense) / 100));
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
            {
                victor = attacker;
                return true;
            }
        }

        return false;
    }

    public bool DoSecondaryPhysicalAttack (Player attacker, Player defender)
    {
        // attacker Attacks

        // Check they hit the person.
        if (random.Next(1, 100) > attacker.hitChance)
        {
            Debug.Log(attacker.username + " took a swing at " + defender.username + " and missed!");
        }
        else if (random.Next(1, 100) <= defender.dodgeChance)
        {
            Debug.Log(defender.username + " dodged an attack from " + attacker.username + "!");
        }
        else if (random.Next(1, 100) <= defender.blockChance)
        {
            Debug.Log(defender.username + " blocked a hit from " + attacker.username + "!");
        }
        // They hit!
        else
        {
            // Calculate the damage dealt.
            int weapDam = random.Next(attacker.minWeaponDamage, attacker.maxWeaponDamage);
            int damage = (int)Math.Floor((weapDam * attacker.physicalMultiplier) * ((float)(100 - defender.defense) / 100));
            damage = (int)Math.Floor(damage * 0.5);
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
            {
                victor = attacker;
                return true;
            }
        }

        return false;
    }
}
