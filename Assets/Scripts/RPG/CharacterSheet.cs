using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        CLASS_SORCERER,
        CLASS_GOBLIN
    };

    private static System.Random rng;
    private GameObject combatPrefab;
    private GameObject avatar;

    private CharacterClass characterClass;

    public int currentHealth;
    public int maxHealth;

    public string name;
    public string boostStatText;

    public int speed;
    public int endurance;
    public int strength;
    public int agility;
    public int intelligence;

    public int bonusSpeed = 0;
    public int bonusEndurance = 0;
    public int bonusStrength = 0;
    public int bonusAgility = 0;
    public int bonusIntelligence = 0;

    public bool selected = true;

    public Item ringLeftEquipped;
    public Item ringRightEquipped;

    public CharacterSheet(CharacterClass c)
    {
        rng = new System.Random();
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
            case CharacterClass.CLASS_GOBLIN:
                name = "Goblin";
                InitStats(2, 1, 2, 3, 2);
                break;
        }
        InitHealth();
    }

    private void InitHealth()
    {
        maxHealth = Constants.HEALTH_PER_ENDURANCE * endurance + Constants.HEALTH_PER_STRENGTH * strength + 10;
        currentHealth = maxHealth;
    }

    private void InitStats(int spe, int end, int str, int agi, int intel)
    {
        speed = spe;
        endurance = end;
        strength = str;
        agility = agi;
        intelligence = intel;
    }

    public int GetTotalPower()
    {
        return speed + endurance + strength + agility + intelligence;
    }

    public void Heal(int amount)
    {
        currentHealth = currentHealth + amount < maxHealth ? currentHealth + amount : maxHealth;
    }

    public void PowerUp()
    {
        switch (rng.Next(1, 6))
        {
            case 1:
                if (speed < 10)
                {
                    speed += 1;
                    boostStatText = "+1 speed";
                }
                break;
            case 2:
                if (endurance < 10)
                {
                    endurance += 1;
                    maxHealth += Constants.HEALTH_PER_ENDURANCE;
                    currentHealth += Constants.HEALTH_PER_ENDURANCE;
                    boostStatText = "+1 endurance";
                }
                break;
            case 3:
                if (strength < 10)
                {
                    strength += 1;
                    maxHealth += Constants.HEALTH_PER_STRENGTH;
                    currentHealth += Constants.HEALTH_PER_STRENGTH;
                    boostStatText = "+1 strength";
                }
                break;
            case 4:
                if (agility < 10)
                {
                    agility += 1;
                    boostStatText = "+1 agility";
                }
                break;
            case 5:
                if (intelligence < 10)
                {
                    intelligence += 1;
                    boostStatText = "+1 intelligence";
                }
                break;
            default:
                break;
         }
    }

    // If the avatar was injured, register those effects.
    // Remember that stamina losses are transient - only health losses last past combat.
    public void HandleCombatEffects()
    {
        // Unit fell in combat. Set to one health.
        if (avatar == null)
        {
            currentHealth = 1;
            return;
        }
        // Otherwise, however many health points the avatar has.
        currentHealth = avatar.GetComponent<CreatureMechanics>().currentHealth;
        currentHealth = currentHealth < 1 ? 1 : currentHealth;
    }

    // Creates the combat avatar for the character in a combat scene.
    // Gives it the appropriate Model, CreatureMechanics, CombatMechanics, Special Moves, etc.
    // Can create as PC or as enemy.
    public void CreateCombatAvatar(Vector3 location, Quaternion rotation, bool asPC)
    {
        combatPrefab = (GameObject)Resources.Load("Prefabs/combatant", typeof(GameObject));
        GameObject combatant = GameObject.Instantiate(combatPrefab, location, rotation) as GameObject;
        if (asPC)
        {
            combatant.AddComponent<PlayerController>();
        }
        else
        {
            combatant.AddComponent<EnemyController>();
        }
        List<Action> specialMoves = new List<Action> { };
        switch (characterClass)
        {
            case CharacterClass.CLASS_HERO:
                combatant.AddComponent<CreatureMechanics>();
                // At some point we may want to replace this with some sort of 'skill learning' system where the unit can learn new skills? But for now just add all the skills.
                specialMoves.Add(combatant.AddComponent<ActionEmpower>());
                specialMoves.Add(combatant.AddComponent<ActionMultiAttack>());
                specialMoves.Add(combatant.AddComponent<ActionOffhandAttack>());
                specialMoves.Add(combatant.AddComponent<ActionLifeOrDeath>());
                break;
            case CharacterClass.CLASS_MEDUSA:
                combatant.GetComponent<CombatController>().SetTileSearchType(CombatController.TileSearchType.DEFAULT_RANGED);
                combatant.AddComponent<MedusaMechanics>();
                // At some point we may want to replace this with some sort of 'skill learning' system where the unit can learn new skills? But for now just add all the skills.
                specialMoves.Add(combatant.AddComponent<ActionPetrify>());
                specialMoves.Add(combatant.AddComponent<ActionSnakeBite>());
                specialMoves.Add(combatant.AddComponent<ActionTerrify>());
                specialMoves.Add(combatant.AddComponent<ActionTailSweep>());
                break;
            case CharacterClass.CLASS_MINOTAUR:
                combatant.AddComponent<CreatureMechanics>();
                // At some point we may want to replace this with some sort of 'skill learning' system where the unit can learn new skills? But for now just add all the skills.
                specialMoves.Add(combatant.AddComponent<ActionBullRush>());
                specialMoves.Add(combatant.AddComponent<ActionRage>());
                specialMoves.Add(combatant.AddComponent<ActionSlaughter>());
                specialMoves.Add(combatant.AddComponent<ActionRegenerate>());
                break;
            case CharacterClass.CLASS_ARCHER:
                combatant.AddComponent<ArcherMechanics>();
                combatant.GetComponent<CombatController>().SetTileSearchType(CombatController.TileSearchType.DEFAULT_RANGED);
                specialMoves.Add(combatant.AddComponent<ActionCrippleShot>());
                specialMoves.Add(combatant.AddComponent<ActionPoisonArrow>());
                specialMoves.Add(combatant.AddComponent<ActionBurningArrow>());
                specialMoves.Add(combatant.AddComponent<ActionFastShot>());
                break;
            case CharacterClass.CLASS_MYRMADON:
                combatant.AddComponent<CreatureMechanics>();
                combatant.GetComponent<CombatController>().SetTileSearchType(CombatController.TileSearchType.DEFAULT_REACH);
                specialMoves.Add(combatant.AddComponent<ActionSweep>());
                specialMoves.Add(combatant.AddComponent<ActionCrippleStrike>());
                specialMoves.Add(combatant.AddComponent<ActionShieldBash>());
                specialMoves.Add(combatant.AddComponent<ActionBulwark>());
                break;
            case CharacterClass.CLASS_SORCERER:
                combatant.AddComponent<CreatureMechanics>();
                combatant.GetComponent<CombatController>().SetTileSearchType(CombatController.TileSearchType.DEFAULT_REACH);
                // At some point we may want to replace this with some sort of 'skill learning' system where the unit can learn new skills? But for now just add all the skills.
                specialMoves.Add(combatant.AddComponent<ActionFreeze>());
                specialMoves.Add(combatant.AddComponent<ActionIgnite>());
                specialMoves.Add(combatant.AddComponent<ActionBloodlust>());
                specialMoves.Add(combatant.AddComponent<ActionOffhandAttack>());
                break;
            case CharacterClass.CLASS_GOBLIN:
                combatant.AddComponent<CreatureMechanics>();
                specialMoves.Add(combatant.AddComponent<ActionPerfidy>());
                specialMoves.Add(combatant.AddComponent<ActionCausticPowder>());
                specialMoves.Add(combatant.AddComponent<ActionKneecap>());
                specialMoves.Add(combatant.AddComponent<ActionMobility>());
                break;
            case CharacterClass.CLASS_SLAVE:
                combatant.AddComponent<CreatureMechanics>();
                break;
        }
        string displayName = asPC ? name : "Enemy " + name;
        avatar = combatant;
        combatant.GetComponent<CreatureMechanics>().Init(maxHealth, currentHealth, speed + bonusSpeed, endurance + bonusEndurance, strength + bonusStrength, agility + bonusAgility, intelligence + bonusIntelligence, displayName);
        combatant.GetComponent<CombatController>().SetSpecialMoves(specialMoves);
    }
}
