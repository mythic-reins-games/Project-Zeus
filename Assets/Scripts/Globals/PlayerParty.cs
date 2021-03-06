﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class PlayerParty
{
    public static List<CharacterSheet> partyMembers;
    public static int gold;
    private static System.Random rng;
    public static List<Item> inventory;

    // Resets the party to only contain the main character/hero.
    public static void Reset()
    {
        partyMembers = new List<CharacterSheet> { };
        inventory = new List<Item> { };
        partyMembers.Add(new CharacterSheet(CharacterSheet.CharacterClass.CLASS_HERO));
        gold = 28;
        rng = new System.Random();
    }

    static int GetTotalPower()
    {
        int pow = 0;
        foreach (CharacterSheet c in partyMembers)
        {
            if (c.selected)
                pow += c.GetTotalPower();
        }
        return pow;
    }

    public static void AfterCombatEffects()
    {
        foreach (CharacterSheet c in partyMembers)
        {
            if (c.selected)
                c.HandleCombatEffects();
        }
    }

    public static int CountActivePartyMembers()
    {
        int count = 0;
        foreach (CharacterSheet c in partyMembers)
        {
            if (c.selected) count++;
        }
        return count;
    }

    public static void AddToParty(CharacterSheet.CharacterClass c)
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
                c.CreateCombatAvatar(new Vector3(xPos++, 0.08f, -5.5f), facing, true);
        }
    }

    private static bool PercentRoll(int percent)
    {
        return rng.Next(0, 99) < percent;
    }

    public static void ResetPowerUpTexts()
    {
        foreach (CharacterSheet c in partyMembers)
        {
            c.boostStatText = null;
        }
    }

    public static void BoostFromVictoriousArenaCombat()
    {
        gold += 15;
        float percent = 100f * (float)EnemyParty.GetTotalPower() / (float)GetTotalPower();
        percent -= 50f;
        foreach (CharacterSheet c in partyMembers)
        {
            if (c.selected && PercentRoll((int)percent))
            {
                c.PowerUp();
            }
        }
    }
}
