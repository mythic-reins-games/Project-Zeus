using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSheet
{
    public enum CharacterClass
    {
        CLASS_HERO,
        CLASS_MEDUSA,
        CLASS_MINOTAUR,
        CLASS_SLAVE,
        CLASS_ARCHER,
        CLASS_MYRMADON,
        CLASS_SORCERER
    };

    public GameObject combatPrefab;

    private CharacterClass characterClass;

    private string name;

    private int speed;
    private int endurance;
    private int strength;
    private int agility;
    private int intelligence;

    public CharacterSheet(CharacterClass c)
    {
        characterClass = c;
        switch (c)
        {
            case CharacterClass.CLASS_HERO:
                name = "Hero";
                InitStats(2, 2, 2, 2, 2);
                break;
            case CharacterClass.CLASS_MEDUSA:
                name = "Medusa";
                InitStats(1, 2, 2, 3, 2);
                break;
            case CharacterClass.CLASS_MINOTAUR:
                name = "Minotaur";
                InitStats(2, 2, 3, 2, 1);
                break;
            case CharacterClass.CLASS_ARCHER:
                name = "Archer";
                InitStats(3, 1, 2, 2, 2);
                break;
            case CharacterClass.CLASS_MYRMADON:
                name = "Myrmadon";
                InitStats(2, 3, 2, 1, 2);
                break;
            case CharacterClass.CLASS_SORCERER:
                name = "Sorcerer";
                InitStats(2, 2, 1, 2, 3);
                break;
            case CharacterClass.CLASS_SLAVE:
                name = "Slave";
                InitStats(1, 1, 1, 1, 1);
                break;
        }
    }

    private void InitStats(int spe, int end, int str, int agi, int intel)
    {
        speed = spe;
        endurance = end;
        strength = str;
        agility = agi;
        intelligence = intel;
    }

    // Creates the combat avatar for the character in a combat scene.
    // Gives it the appropriate Model, CreatureMechanics, CombatMechanics, Special Moves, etc.
    // Can create as PC or as enemy.
    public void CreateCombatAvatar(Vector3 location, Quaternion rotation, bool asPC)
    {
        GameObject manager = GameObject.Find("CombatManager");
        GameObject combatant = GameObject.Instantiate(combatPrefab, location, rotation, manager.transform) as GameObject;
        if (asPC)
        {
            combatant.AddComponent("PlayerController");
        }
        else
        {

        }
        switch (characterClass)
        {
            case CharacterClass.CLASS_HERO:
                break;
            case CharacterClass.CLASS_MEDUSA:
                combatant.GetComponent<CombatController>().SetTileSearchType(CombatController.TileSearchType.DEFAULT_RANGED);
                break;
            case CharacterClass.CLASS_MINOTAUR:
                break;
            case CharacterClass.CLASS_ARCHER:
                combatant.GetComponent<CombatController>().SetTileSearchType(CombatController.TileSearchType.DEFAULT_RANGED);
                break;
            case CharacterClass.CLASS_MYRMADON:
                break;
            case CharacterClass.CLASS_SORCERER:
                break;
            case CharacterClass.CLASS_SLAVE:
                break;
        }
    }
}
