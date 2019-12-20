using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyParty
{
    public static List<CharacterSheet> partyMembers;

    public static int difficulty = 1;

    private static System.Random rng;
    
    public static void Reset()
    {
        rng = new System.Random();
    }

    private static int DifficultyJitter()
    {
        return rng.Next(-1, 2);
    }

    private static CharacterSheet.CharacterClass RandomNormalClass()
    {
        switch(rng.Next(1, 8))
        {
            case 1:
                return CharacterSheet.CharacterClass.CLASS_MINOTAUR;
            case 2:
                return CharacterSheet.CharacterClass.CLASS_HERO;
            case 3:
                return CharacterSheet.CharacterClass.CLASS_MEDUSA;
            case 4:
                return CharacterSheet.CharacterClass.CLASS_GOBLIN;
            case 5:
                return CharacterSheet.CharacterClass.CLASS_SORCERER;
            case 6:
                return CharacterSheet.CharacterClass.CLASS_ARCHER;
            case 7:
                return CharacterSheet.CharacterClass.CLASS_MYRMADON;
        }
        return CharacterSheet.CharacterClass.CLASS_SLAVE;
    }

    // Sets up an enemy party at a particular diffulty.
    // Any old/obsolete enemy party is overwritten.
    public static void SetArenaFoes()
    {
        partyMembers = new List<CharacterSheet> { };
        switch(difficulty + DifficultyJitter())
        {
            case 0:
                AddToParty(CharacterSheet.CharacterClass.CLASS_SLAVE);
                break;
            case 1:
                AddToParty(RandomNormalClass());
                break;
            case 2:
                AddToParty(CharacterSheet.CharacterClass.CLASS_SLAVE);
                AddToParty(RandomNormalClass());
                break;
            case 3:
                AddToParty(CharacterSheet.CharacterClass.CLASS_SLAVE);
                AddToParty(CharacterSheet.CharacterClass.CLASS_SLAVE);
                AddToParty(RandomNormalClass());
                break;
            case 4:
                AddToParty(CharacterSheet.CharacterClass.CLASS_SLAVE);
                AddToParty(RandomNormalClass());
                AddToParty(RandomNormalClass());
                break;
            case 5:
                AddToParty(CharacterSheet.CharacterClass.CLASS_SLAVE);
                AddToParty(CharacterSheet.CharacterClass.CLASS_SLAVE);
                AddToParty(RandomNormalClass());
                AddToParty(RandomNormalClass());
                break;
            default:
                AddToParty(CharacterSheet.CharacterClass.CLASS_SLAVE);
                AddToParty(RandomNormalClass());
                AddToParty(RandomNormalClass());
                AddToParty(RandomNormalClass());
                break;
        }
        for (int i = 7; i < difficulty; i += 2)
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
        Quaternion facing = new Quaternion(0f, 180f, 0f, 0f);
        foreach (CharacterSheet c in partyMembers)
        {
            if (c.selected)
            {
                c.CreateCombatAvatar(new Vector3(xPos++, 0.08f, 5.5f), facing, false);
            }
        }
    }
}