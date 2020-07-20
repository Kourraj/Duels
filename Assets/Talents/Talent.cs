using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TalentTier
{
    Tier1,
    Tier2,
    Tier3
}

[CreateAssetMenu (fileName="New Talent", menuName="Character Attributes/Talent")]
public class Talent : ScriptableObject
{
    [Header("Base Info")]
    public string id;
    public string talentName;
    [TextArea]
    public string talentDescription;

    [Space]

    [Header("Conditions")]
    public TalentTier talentTier;
    public ClassType classType;

    public bool CanEquip(Player player, Talent talent)
    {
        return (player.classType == classType);
    }
}
