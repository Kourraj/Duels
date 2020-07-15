using System;
using UnityEngine;
using UnityEngine.UI;

public class Duel : MonoBehaviour
{
    public Player attacker;
    public Player defender;

    System.Random random;

    int duelDamageMult = 1;
    public int randomSeed = 5;

    // UI
    public GameObject content;
    public Scrollbar VScrollbar;
    public GameObject baseText;

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
            AddText("Turn " + turnCount);

            // Actions
            if (DoAction(first, second))
                break;
            if (DoAction(second, first))
                break;

            if (turnCount == 1)
            {
                // Skills
                DoSkill(first);
                DoSkill(second);
            } 
            first.skillTurns -= 1;
            second.skillTurns -= 1;
            if (first.skillTurns == 0)
                EndSkill(first);
            if (second.skillTurns == 0)
                EndSkill(second);

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
                    AddText(second.username + " bleeds for " + bleedDmg + " damage.");
                    AddText(second.username + " is on " + second.currentHP + "❤");
                }
                else if (turnCount % 2 == 0)
                {
                    int turnDmg = 50 + ((turnCount - 16) * 10);
                    int bleedDmg = random.Next(turnDmg - (turnDmg / 10), turnDmg + (turnDmg / 10));
                    AddText(first.username + " bleeds for " + bleedDmg + " damage.");
                    AddText(first.username + " is on " + first.currentHP + "❤");
                }
            }
        }

        // Victory stuff
        AddText("And the winner is.... " + victor.username);
        AddText(turnCount);
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

    Player DoAction(Player attacker, Player defender)
    {
        return null;
    }
    void DoSkill(Player attacker)
    {
        // Warrior Skills
        if (attacker.playerClass == PlayerClass.Warrior)
        {
            // Go through each skill (that applies buffs)
            switch (attacker.skill.id)
            {
                case "Arms":
                    attacker.strength = (int)(attacker.strength * 1.25);
                    attacker.CalcualteStats();
                    AddText(attacker.username + " flexed their arms!");
                    break;

                case "LowStance":
                    attacker.defense += (int)(attacker.defense * 0.425);
                    attacker.dodgeChance += 15;
                    attacker.skillTurns = attacker.stamina / 8;
                    break;

                case "BarbarianBattleCry":
                    attacker.resist += (int)(attacker.resist * 0.40);
                    attacker.defense += (int)(attacker.defense * 0.15);
                    attacker.CalcualteStats();
                    attacker.skillTurns = (int)Math.Ceiling((decimal)(attacker.stamina / 30));
                    break;

                case "ShieldSlam":
                    attacker.blockChance += 12;
                    attacker.dodgeChance -= 7;
                    break;

                case "FortifiedArmour":
                    attacker.armour += (int)(attacker.armour * 0.25);
                    attacker.resist += (int)(attacker.resist * 0.15);
                    attacker.CalcualteStats();
                    break;

                default:
                    AddText("No (de)buff bases skill.");
                    break;
            }
        } 
    }

    void EndSkill(Player attacker)
    {
        // Warrior Skills
        if (attacker.playerClass == PlayerClass.Warrior)
        {
            // Go through each skill (that applies buffs)
            switch (attacker.skill.id)
            {
                case "LowStance":
                    attacker.defense = (int)(attacker.defense / 1.425);
                    attacker.dodgeChance -= 15;
                    break;

                case "BarbarianBattleCry":
                    attacker.resist = (int)(attacker.resist / 1.40);
                    attacker.defense = (int)(attacker.defense / 1.15);
                    attacker.CalcualteStats();
                    break;
                    
                default:
                    AddText("No (de)buff bases skill.");
                    break;
            }
        }
    }

    Player DoMainHandAttack(Player attacker, Player defender)
    {
        // attacker Attacks

        // Check they hit the person.
        if (random.Next(1, 100) > attacker.hitChance)
        {
            AddText(attacker.username + " took a swing at " + defender.username + " and missed!");
        }
        else if (random.Next(1, 100) <= defender.blockChance)
        {
            AddText(defender.username + " blocked a hit from " + attacker.username + "!");
        }
        else if (random.Next(1, 100) <= defender.dodgeChance)
        {
            AddText(defender.username + " dodged an attack from " + attacker.username + "!");
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
                    AddText(attacker.username + " smashed " + defender.username + " for a SUPER critical hit! They dealt " + damage.ToString() + " damage!");
                    defender.currentHP -= damage;
                    AddText(defender.username + " is on " + defender.currentHP + "❤");
                }
                else
                {
                    // Critical Hit!
                    damage = (int)Math.Floor((damage / ((float)(100 - defender.defense) / 100)) * 1.5);
                    AddText(attacker.username + " smacked " + defender.username + " for a critical hit! They dealt " + damage.ToString() + " damage!");
                    defender.currentHP -= damage;
                    AddText(defender.username + " is on " + defender.currentHP + "❤");
                }
            }
            else
            {
                // Just a normal hit.
                AddText(attacker.username + " hit " + defender.username + " for " + damage.ToString() + " damage!");
                defender.currentHP -= damage;
                AddText(defender.username + " is on " + defender.currentHP + "❤");
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
            AddText(attacker.username + " took a swing at " + defender.username + " and missed!");
        }
        else if (random.Next(1, 100) <= defender.blockChance)
        {
            AddText(defender.username + " blocked a hit from " + attacker.username + "!");
        }
        else if (random.Next(1, 100) <= defender.dodgeChance)
        {
            AddText(defender.username + " dodged an attack from " + attacker.username + "!");
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
                    AddText(attacker.username + " smashed " + defender.username + " with their secondary weapon for a SUPER critical hit! They dealt " + damage.ToString() + " damage!");
                    defender.currentHP -= damage;
                    AddText(defender.username + " is on " + defender.currentHP + "❤");
                }
                else
                {
                    // Critical Hit!
                    damage = (int)Math.Floor((damage / ((float)(100 - defender.defense) / 100)) * 1.5);
                    AddText(attacker.username + " smacked " + defender.username + " with their secondary weapon for a critical hit! They dealt " + damage.ToString() + " damage!");
                    defender.currentHP -= damage;
                    AddText(defender.username + " is on " + defender.currentHP + "❤");
                }
            }
            else
            {
                // Just a normal hit.
                AddText(attacker.username + " hit " + defender.username + " with their secondary weapon for " + damage.ToString() + " damage!");
                defender.currentHP -= damage;
                AddText(defender.username + " is on " + defender.currentHP + "❤");
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
            AddText(attacker.username + " took a swing at " + defender.username + " and missed!");
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
                    AddText(attacker.username + " magicked " + defender.username + " for a SUPER critical hit! They dealt " + damage.ToString() + " damage!");
                    defender.currentHP -= damage;
                    AddText(defender.username + " is on " + defender.currentHP + "❤");
                }
                else
                {
                    // Critical Hit!
                    damage = (int)Math.Floor(damage * 1.5);
                    AddText(attacker.username + " magicked " + defender.username + " for a critical hit! They dealt " + damage.ToString() + " damage!");
                    defender.currentHP -= damage;
                    AddText(defender.username + " is on " + defender.currentHP + "❤");
                }
            }
            else
            {
                // Check if it was resisted
                if (random.Next(1, 100) <= defender.resistanceChance)
                {
                    AddText(defender.username + " resisted an attack from " + attacker.username + "!");
                }
                // Just a normal hit.
                else
                {
                    AddText(attacker.username + " magicked " + defender.username + " for " + damage.ToString() + " damage!");
                    defender.currentHP -= damage;
                    AddText(defender.username + " is on " + defender.currentHP + "❤");
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
                    if (first.offHand != null)
                    {
                        if (!(first.offHand.weaponType == WeaponType.Wand))
                            if (DoOffHandAttack(first, second))
                                return first;
                        // Wand offhand
                        if (first.offHand.weaponType == WeaponType.Wand)
                            if (DoOffHandWandAttack(first, second))
                                return first;
                    }

                firstAttacksMade += 1;
            }
            else
            {
                if ((secondAttacksMade % 2) == 0 || second.offHand == null)
                    if (DoMainHandAttack(second, first))
                        return second;
                else
                    if (second.offHand != null)
                    {
                        if (!(second.offHand.weaponType == WeaponType.Wand))
                            if (DoOffHandAttack(second, first))
                                return second;
                        // Wand offhand
                        if (second.offHand.weaponType == WeaponType.Wand)
                            if (DoOffHandWandAttack(second, first))
                                return second;
                    }

                secondAttacksMade += 1;
            }

        }
        return null;
    }

    void AddText(string text)
    {
        Vector3 position = new Vector3(0f, 0f, 0f);
        Quaternion rotation = new Quaternion(0f, 0f, 0f, 0f);
        Debug.Log(text);
        //GameObject newText = (GameObject)Instantiate(baseText, position, rotation);
        //newText.GetComponent<TMPText>();
    }
}
