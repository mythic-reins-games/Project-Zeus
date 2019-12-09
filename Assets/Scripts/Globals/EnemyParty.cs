using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyParty
{
    public static List<CharacterSheet> partyMembers;

    // Sets up an enemy party at a particular diffulty.
    // Any old/obsolete enemy party is overwritten.
    public static void SetArenaFoes(int difficulty)
    {
        partyMembers = new List<CharacterSheet> { };
        AddToParty(CharacterSheet.CharacterClass.CLASS_SLAVE);
        AddToParty(CharacterSheet.CharacterClass.CLASS_SLAVE);
    }

    private static void AddToParty(CharacterSheet.CharacterClass c)
    {
        partyMembers.Add(new CharacterSheet(c));
    }
}