using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerParty
{
    public static List<CharacterSheet> partyMembers;

    // Resets the party to only contain the main character/hero.
    public static void Reset()
    {
        partyMembers = new List<CharacterSheet> { };
        partyMembers.Add(new CharacterSheet(CharacterSheet.CharacterClass.CLASS_HERO));
    }

    public static void AddToParty(CharacterSheet.CharacterClass c)
    {
        partyMembers.Add(new CharacterSheet(c));
    }
}
