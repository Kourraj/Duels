using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class Duel : MonoBehaviour
{
    public Player attacker;
    public Player defender;

    System.Random random;

    int duelDamageMult = 1;
    public int randomSeed = 5;

    // UI
    public Transform content;
    public Scrollbar VScrollbar;
    public GameObject baseText;
    public int textDelay;

    Queue<string> textQueue = new Queue<string>();
    bool textUpdating = false;

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
            turnCount++;
            // Caps turn count, if no damage happens, the duel won't go on forever.
            if (turnCount >= 100)
                break;
            // Display the turn
            AddText("<style=\"Turn\">Turn " + turnCount);

            // Actions
            if (DoAction(first, second))
                break;
            if (DoAction(second, first))
                break;

            if (turnCount == 1)
            {
                // Skills
                victor = DoSkill(first);
                if (victor != null)
                    break;
                victor = DoSkill(second);
                if (victor != null)
                    break;
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
                    AddText(second.username + " is on " + second.currentHP + "<sprite index=0>");
                }
                else if (turnCount % 2 == 0)
                {
                    int turnDmg = 50 + ((turnCount - 16) * 10);
                    int bleedDmg = random.Next(turnDmg - (turnDmg / 10), turnDmg + (turnDmg / 10));
                    AddText(first.username + " bleeds for " + bleedDmg + " damage.");
                    AddText(first.username + " is on " + first.currentHP + "<sprite index=0>");
                }
            }
        }

        // Victory stuff
        AddText("And the winner is.... " + victor.username);
        AddText(turnCount.ToString());
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
    Player DoSkill(Player attacker)
    {
        // Make sure we actually have a skill.
        if ((bool)attacker.skill) return null;

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
                    attacker.defence += (int)(attacker.defence * 0.425);
                    attacker.dodgeChance += 15;
                    attacker.skillTurns = attacker.stamina / 8;
                    break;

                case "Headbutt":
                    attacker.skillTurns = (int)Math.Ceiling((decimal)(defender.armour / 50));
                    break;

                case "BarbarianBattleCry":
                    attacker.resistanceChance = (int)(attacker.resistanceChance * 1.40);
                    attacker.defence = (int)(attacker.defence * 1.15);
                    attacker.skillTurns = (int)Math.Ceiling((decimal)(attacker.stamina / 30));
                    break;

                case "Charge":
                    defender.criticalStrikeChance -= (int)((attacker.stamina * 0.1) / 100);
                    defender.dodgeChance -= (int)((attacker.stamina * 0.1) / 100);
                    attacker.skillTurns = (int)Math.Ceiling((decimal)(defender.speed / 100));

                    #region QuickAttacks

                    int firstAttackCount = CalculateAttacks(attacker);
                    int firstAttacksMade = 0;
                    // Attacks

                    while (firstAttackCount != firstAttacksMade)
                    {
                        // Normal attacks
                        if ((firstAttacksMade % 2) == 0 || attacker.offHand == null)
                            if (DoMainHandAttack(attacker, defender))
                                return attacker;
                            else
                            if (attacker.offHand != null)
                            {
                                if (!(attacker.offHand.weaponType == WeaponType.Wand))
                                    if (DoOffHandAttack(attacker, defender))
                                        return attacker;
                                // Wand offhand
                                if (attacker.offHand.weaponType == WeaponType.Wand)
                                    if (DoOffHandWandAttack(attacker, defender))
                                        return attacker;
                            }
                    }
                    #endregion QuickAttacks

                    break;

                case "ShieldSlam":
                    attacker.blockChance += 12;
                    attacker.dodgeChance -= 7;
                    break;

                case "FortifiedArmour":
                    attacker.armour = (int)(attacker.armour * 1.25);
                    attacker.CalcualteStats();
                    attacker.resistanceChance = (int)(attacker.resistanceChance * 1.15);
                    break;

                default:
                    AddText("No (de)buff bases skill.");
                    break;
            }
        }
        // Mage Skills
        else if (attacker.playerClass == PlayerClass.Mage)
        {
            // Go through each skill (that applies buffs)
            switch (attacker.skill.id)
            {
                case "BloodMastery":
                    attacker.resist = (int)(attacker.resist * 1.25);
                    attacker.CalcualteStats();
                    attacker.maxHP = (int)(attacker.maxHP * 1.25);
                    attacker.currentHP = attacker.maxHP - (attacker.maxHP - attacker.currentHP);
                    break;

                case "Charge":
                    defender.criticalStrikeChance += (int)((attacker.stamina * 0.1) / 100);
                    defender.dodgeChance += (int)((attacker.stamina * 0.1) / 100);
                    break;

                default:
                    AddText("No (de)buff bases skill.");
                    break;
            }
        }

        return null;
    }

    void EndSkill(Player attacker)
    {
        // Make sure we actually have a skill.
        if (attacker.skill) return;

        // Warrior Skills
        if (attacker.playerClass == PlayerClass.Warrior)
        {
            // Go through each skill (that applies buffs)
            // TODO - End all buff based skills.
            switch (attacker.skill.id)
            {
                case "LowStance":
                    attacker.defence = (int)(attacker.defence / 1.425);
                    attacker.dodgeChance -= 15;
                    break;

                case "BarbarianBattleCry":
                    attacker.resistanceChance = (int)(attacker.resistanceChance / 1.40);
                    attacker.defence = (int)(attacker.defence / 1.15);
                    break;

                default:
                    AddText("No (de)buff bases skill.");
                    break;
            }
        }
    }

    Player DoMainHandAttack(Player attacker, Player defender)
    {
        // Apply T1 skills
        attacker.ApplyT1Talents(attacker.mainHand);

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
            int damage = (int)Math.Floor((weapDam * attacker.physicalMultiplier) * ((float)(100 - defender.defence) / 100));
            damage *= duelDamageMult;
            if (attacker.offHand == null && (attacker.tier2Talent && attacker.tier2Talent.id == "Two-HandedWeapon"))
                damage = (int)(damage * 1.25);
            if (random.Next(1, 100) < attacker.criticalStrikeChance || ((attacker.skill && attacker.skill.id == "Sledgehammer") && random.Next(1, 100) < attacker.criticalStrikeChance))
            {
                if (random.Next(1, 100) < 20)
                {
                    // SUPER CRITICAL!
                    // Double original damage and ignores defence.
                    damage = (int)Math.Floor((damage / ((float)(100 - defender.defence) / 100)) * 2);
                    // Iron Man Skill
                    if (attacker.skill && attacker.skill.id == "IronMan")
                    {
                        damage = (int)(damage * 0.7);
                        if (random.Next(1, 100) <= 25)
                        {
                            // Attacker hit the reflection!
                            AddText(defender.username + " activated Iron Man and reflects the critical back!");
                            AddText(defender.username + " smashed " + attacker.username + " for a SUPER critical hit! They dealt " + damage.ToString() + " damage!");
                            attacker.currentHP -= damage;
                            AddText(attacker.username + " is on " + attacker.currentHP + "<sprite index=0>");

                            // We need to end the attack here, so we don't deal any more damage ot send any more text.
                            if (defender.currentHP <= 0)
                            {
                                attacker.RemoveT1Talents(attacker.mainHand);
                                return attacker;
                            }
                            attacker.RemoveT1Talents(attacker.mainHand);
                            return null;
                        }
                    }
                    // Continue with a normal SUPER CRITICAL
                    AddText(attacker.username + " smashed " + defender.username + " for a SUPER critical hit! They dealt " + damage.ToString() + " damage!");
                    defender.currentHP -= damage;
                    AddText(defender.username + " is on " + defender.currentHP + "<sprite index=0>");
                }
                else
                {
                    // Critical Hit!
                    damage = (int)Math.Floor((damage / ((float)(100 - defender.defence) / 100)) * 1.5);
                    AddText(attacker.username + " smacked " + defender.username + " for a critical hit! They dealt " + damage.ToString() + " damage!");
                    defender.currentHP -= damage;
                    AddText(defender.username + " is on " + defender.currentHP + "<sprite index=0>");
                }

                if ((attacker.skill && attacker.skill.id == "Sledgehammer") && defender.defence > 30)
                {
                    defender.armour = (int)(defender.armour * 0.97);
                    defender.CalcualteStats();
                    AddText("Thanks to " + attacker.username + "'s skill, " + attacker.skill.skillName + " " + defender.username + " lost 3% of their armour!");
                }
            }
            else
            {
                // Just a normal hit.
                AddText(attacker.username + " hit " + defender.username + " for " + damage.ToString() + " damage!");
                defender.currentHP -= damage;
                AddText(defender.username + " is on " + defender.currentHP + "<sprite index=0>");
            }

            // Did they die?
            if (defender.currentHP <= 0)
            {
                attacker.RemoveT1Talents(attacker.mainHand);
                return defender;
            }
        }

        attacker.RemoveT1Talents(attacker.mainHand);
        return null;
    }

    Player DoOffHandAttack(Player attacker, Player defender)
    {
        // attacker Attacks

        // Check they hit the person.

        if (random.Next(1, 100) > (attacker.hitChance * .5) || ((attacker.tier2Talent && attacker.tier2Talent.id == "Ambidextrous") && random.Next(1, 100) > attacker.hitChance * 0.75))
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
            int damage = (int)Math.Floor((weapDam * attacker.physicalMultiplier) * ((float)(100 - defender.defence) / 100));
            if (attacker.tier2Talent && attacker.tier2Talent.id == "Ambidextrous")
                damage = (int)Math.Floor(damage * 0.75);
            else
                damage = (int)Math.Floor(damage * 0.5);
            damage *= duelDamageMult;
            if (random.Next(1, 100) < attacker.criticalStrikeChance)
            {
                if (random.Next(1, 100) < 20)
                {
                    // SUPER CRITICAL!
                    // Double original damage and ignores defence.
                    damage = (int)Math.Floor((damage / ((float)(100 - defender.defence) / 100)) * 2);
                    AddText(attacker.username + " smashed " + defender.username + " with their secondary weapon for a SUPER critical hit! They dealt " + damage.ToString() + " damage!");
                    defender.currentHP -= damage;
                    AddText(defender.username + " is on " + defender.currentHP + "<sprite index=0>");
                }
                else
                {
                    // Critical Hit!
                    damage = (int)Math.Floor((damage / ((float)(100 - defender.defence) / 100)) * 1.5);
                    AddText(attacker.username + " smacked " + defender.username + " with their secondary weapon for a critical hit! They dealt " + damage.ToString() + " damage!");
                    defender.currentHP -= damage;
                    AddText(defender.username + " is on " + defender.currentHP + "<sprite index=0>");
                }
            }
            else
            {
                // Just a normal hit.
                AddText(attacker.username + " hit " + defender.username + " with their secondary weapon for " + damage.ToString() + " damage!");
                defender.currentHP -= damage;
                AddText(defender.username + " is on " + defender.currentHP + "<sprite index=0>");
            }

            // Did they die?
            if (defender.currentHP <= 0)
            {
                attacker.RemoveT1Talents(attacker.mainHand);
                return attacker;
            }
        }

        attacker.RemoveT1Talents(attacker.mainHand);
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
                    damage += damage;
                    AddText(attacker.username + " magicked " + defender.username + " for a SUPER critical hit! They dealt " + damage.ToString() + " damage!");
                    defender.currentHP -= damage;
                    AddText(defender.username + " is on " + defender.currentHP + "<sprite index=0>");
                }
                else
                {
                    // Critical Hit!
                    damage = (int)Math.Floor(damage * 1.5);
                    AddText(attacker.username + " magicked " + defender.username + " for a critical hit! They dealt " + damage.ToString() + " damage!");
                    defender.currentHP -= damage;
                    AddText(defender.username + " is on " + defender.currentHP + "<sprite index=0>");
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
                    AddText(defender.username + " is on " + defender.currentHP + "<sprite index=0>");
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

        while (firstAttackCount != firstAttacksMade || secondAttackCount != secondAttacksMade)
        {
            // First's turn to attack.
            if (firstAttacksMade == secondAttacksMade || secondAttackCount == secondAttacksMade)
            {
                // Talent Checks.
                if (second.skillTurns != 0 && (second.skill && second.skill.id == "Headbutt"))
                {
                    if (random.Next(1, 100) <= first.resistanceChance)
                        AddText(first.username + " resisted a headbutt from " + second.username + ".");
                    else
                    {
                        secondAttacksMade--;
                        AddText(second.username + " headbutt " + first.username + "! They lose a turn and " + second.username + " gains one!");
                        // Pass a while run/skip to second's attack
                    }
                }
                // Normal attacks
                else if ((firstAttacksMade % 2) == 0 || first.offHand == null)
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

    public void AddText(string text)
    {
        // TODO - Allow for text types.
        // Adds the text to the text queue.
        textQueue.Enqueue(text);
    }

    void Update()
    {
        if (!textUpdating && textQueue.Count != 0)
        {
            textUpdating = true;
            StartCoroutine(UpdateText());
        }
    }

    IEnumerator UpdateText()
    {
        while (textQueue.Count != 0)
        {
            // Instantiate a new text object.
            // Transform doesn't matter as a layout component of content does that
            Vector3 position = new Vector3(0f, 0f, 0f);
            Quaternion rotation = new Quaternion(0f, 0f, 0f, 0f);
            GameObject newText = (GameObject)Instantiate(baseText, position, rotation, content);

            // Get the text part of the TMP Object and set it to the desired value.
            TMP_Text textComponent = newText.GetComponent<TMP_Text>();
            textComponent.text = textQueue.Dequeue();

            // Wait for three seconds before 
            yield return new WaitForSeconds(textDelay);
        }
        textUpdating = false;
    }
}
