using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName="New Skill", menuName="Character Attributes/Skill")]
public class Skill : ScriptableObject
{
    [Header("Base Info")]
    public string id;
    public string skillName;
    [TextArea]
    public string skillDescription;

    [Space]

    [Header("Conditions")]
    public PlayerClass playerClass;

    public bool CanEquip(Player player, Skill skill)
    {
        return (player.playerClass == playerClass);
    }
}
