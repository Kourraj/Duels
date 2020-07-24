using System;

using UnityEngine;

using TMPro;

public class DisplayAPlayerStats : MonoBehaviour
{
    [Tooltip("Leave blank if the main player")]
    public Player player;

    public TMP_Text playerName;
    public TMP_Text mainInfo;
    public TMP_Text baseStats;
    public TMP_Text endStats;
    public TMP_Text powerStat;

    public void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("Player").GetComponent<Player>();
        }

        // Set's the player's username
        // Formatting should be predefined.
        playerName.text += player.username;

        // Add key info
        mainInfo.text = "Level " + player.playerLevel + " " + player.playerClass.ToString();
        mainInfo.text += " - <sprite index=0> " + player.maxHP;
        // Calculate and displayer the player's estimated power.
        // TODO - Could do better here
        int damage = (player.mainHand.maxDamage + player.mainHand.minDamage) / 2;
        damage = (int)Math.Floor((damage * player.physicalMultiplier));
        mainInfo.text += "\nEstimated Damage: " + damage;

        // Add the base stats
        baseStats.text = "<sprite=\"EmojiOne\" index=0>" + player.strength;
        baseStats.text += " <sprite=\"EmojiOne\" index=1>" + player.speed;
        baseStats.text += " <sprite=\"EmojiOne\" index=2>" + player.perception;
        baseStats.text += " <sprite=\"EmojiOne\" index=3>" + player.intelligence;
        baseStats.text += "\n<sprite=\"EmojiOne\" index=4>" + player.stamina;
        baseStats.text += " <sprite=\"EmojiOne\" index=5>" + player.resist;
        baseStats.text += " <sprite=\"EmojiOne\" index=6>" + player.armour;

        // End stats
        endStats.text = "<sprite=\"EmojiOne\" index=0>" + player.initiative;
        endStats.text += " <sprite=\"EmojiOne\" index=1>" + player.defence;
        endStats.text += " <sprite=\"EmojiOne\" index=2>" + String.Format("{0:0.00}", player.physicalMultiplier);
        endStats.text += " <sprite=\"EmojiOne\" index=3>" + String.Format("{0:0.00}", player.magicalMultiplier);
        endStats.text += "\n<sprite=\"EmojiOne\" index=4>" + player.hitChance + "%";
        endStats.text += " <sprite=\"EmojiOne\" index=5>" + player.criticalStrikeChance + "%";
        endStats.text += " <sprite=\"EmojiOne\" index=6>" + player.dodgeChance + "%";
        endStats.text += " <sprite=\"EmojiOne\" index=7>" + player.blockChance + "%";
        endStats.text += " <sprite=\"EmojiOne\" index=8>" + player.resistanceChance + "%";

        // Grant a power rating:
        int playerPower = player.CalculatePower();
        powerStat.text = "<style=\"H1\">" + playerPower;
    }
}
