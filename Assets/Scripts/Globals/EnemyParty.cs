using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyParty
{
    public static List<CharacterSheet> partyMembers;

    public static int difficulty = 1;

    // Sets up an enemy party at a particular diffulty.
    // Any old/obsolete enemy party is overwritten.
    public static void SetArenaFoes()
    {
        partyMembers = new List<CharacterSheet> { };
        switch(difficulty)
        {
            case 1:
                AddToParty(CharacterSheet.CharacterClass.CLASS_SLAVE);
                AddToParty(CharacterSheet.CharacterClass.CLASS_MINOTAUR);
                break;
            case 2:
                AddToParty(CharacterSheet.CharacterClass.CLASS_SLAVE);
                AddToParty(CharacterSheet.CharacterClass.CLASS_SLAVE);
                AddToParty(CharacterSheet.CharacterClass.CLASS_HERO);
                break;
            case 3:
                AddToParty(CharacterSheet.CharacterClass.CLASS_SLAVE);
                AddToParty(CharacterSheet.CharacterClass.CLASS_MEDUSA);
                AddToParty(CharacterSheet.CharacterClass.CLASS_MINOTAUR);
                break;
            case 4:
                AddToParty(CharacterSheet.CharacterClass.CLASS_SLAVE);
                AddToParty(CharacterSheet.CharacterClass.CLASS_SLAVE);
                AddToParty(CharacterSheet.CharacterClass.CLASS_MEDUSA);
                AddToParty(CharacterSheet.CharacterClass.CLASS_HERO);
                break;
            default:
                AddToParty(CharacterSheet.CharacterClass.CLASS_SLAVE);
                AddToParty(CharacterSheet.CharacterClass.CLASS_MINOTAUR);
                AddToParty(CharacterSheet.CharacterClass.CLASS_MEDUSA);
                AddToParty(CharacterSheet.CharacterClass.CLASS_HERO);
                break;
        }
        for (int i = 5; i < difficulty; i++)
        {
            foreach (CharacterSheet c in partyMembers)
            {
                c.PowerUp();
            }
        }
    }

    public static int GetTotalPower()
    {
        int pow = 0;
        foreach (CharacterSheet c in partyMembers)
        {
            pow += c.GetTotalPower();
        }
        return pow;
    }

    private static void AddToParty(CharacterSheet.CharacterClass c)
    {
        partyMembers.Add(new CharacterSheet(c));
    }

    public static void SpawnPartyMembers()
    {
        float xPos = -2.5f;
        Quaternion facing = new Quaternion(0f, 0f, 0f, 0f);
        foreach (CharacterSheet c in partyMembers)
        {
            if (c.selected)
            {
                c.CreateCombatAvatar(new Vector3(xPos++, 0.08f, 5.5f), facing, false);
            }
        }
    }
}