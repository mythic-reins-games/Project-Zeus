﻿#pragma warning disable 0649

using UnityEngine;
using System.Linq;

public class CreatureStats : ObjectStats
{
    System.Random rng;

    [SerializeField] GameObject staminaBar;
    IndicatorBar staminaBarScript;

    [SerializeField] private int strength = 10;
    [SerializeField] private int speed = 10;
    [SerializeField] private int endurance = 10;
    [SerializeField] private int agility = 10;
    [SerializeField] private int intelligence = 10;
    [SerializeField] private int maxConcentration = 10;
    [SerializeField] private int concentration = 2;

    [SerializeField] public string name = "";

    protected int maxStamina = 1;
    protected int currentStamina = 1;

    // Start is called before the first frame update
    override protected void Start()
    {
        rng = new System.Random();
        healthBarScript = healthBar.GetComponent<IndicatorBar>();
        staminaBarScript = staminaBar.GetComponent<IndicatorBar>();
        maxHealth = endurance + (strength / 5) + 5;
        maxStamina = endurance + 10;
        currentStamina = maxStamina;
        currentHealth = maxHealth;
        base.Start();
    }

    public string StaminaString()
    {
        return currentStamina + "/" + maxStamina;
    }

    // Average of strength and strength times lifepercent
    protected int GetEffectiveStrength()
    {
        float[] ar = new float[2] { strength, strength * PercentHealth() };
        return Mathf.RoundToInt(ar.Average());
    }

    // Average of agility and agility times lifepercent
    protected int GetEffectiveAgility()
    {
        float[] ar = new float[2] { agility, agility * PercentHealth() };
        return Mathf.RoundToInt(ar.Average());
    }

    // Average of intelligence and intelligence times lifepercent
    protected int GetEffectiveIntelligence()
    {
        float[] ar = new float[2] { intelligence, intelligence * PercentHealth() };
        return Mathf.RoundToInt(ar.Average());
    }

    // Average of Speed and Speed times lifepercent
    protected int GetEffectiveSpeed()
    {
        float[] ar = new float[2] { speed, speed * PercentHealth() };
        return Mathf.RoundToInt(ar.Average());
    }

    private float PercentStamina()
    {
        return (float)currentStamina / (float)maxStamina;
    }

    public int GetMaxActionPoints()
    {
        return 5 + GetEffectiveSpeed() / 10;
    }

    public float GetConcentrationPoints()
    {
        return (float)concentration / (float)maxConcentration;
    }

    override public void ReceiveDamage(int amount)
    {
        if (currentStamina >= amount)
        {
            currentStamina -= amount;
        }
        else
        {
            currentHealth -= (amount - currentStamina);
            currentStamina = 0;
        }
        healthBarScript.SetPercent(PercentHealth());
        staminaBarScript.SetPercent(PercentStamina());
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            Animate("IsGettingDamaged");
        }
    }

    public int HitChance()
    {
        return 75 + (GetEffectiveAgility() / 4);
    }

    override public int DodgeChance()
    {
        return 0 + (GetEffectiveAgility() / 2);
    }

    private bool PercentRoll(int percent)
    {
        return rng.Next(0, 99) < percent;
    }

    public int MaxDamage()
    {
        return 11 + GetEffectiveStrength() / 2;
    }

    public int MinDamage()
    {
        return 1 + GetEffectiveStrength() / 2;
    }

    // Damage from a basic attack is 1-11 plus half of strength.
    private int DamageInflicted()
    {
        return rng.Next(MinDamage(), MaxDamage());
    }

    public int BonusRearDamageMin()
    {
        return 1 + GetEffectiveAgility() / 4;
    }

    public int BonusRearDamageMax()
    {
        return 5 + GetEffectiveAgility() / 4;
    }

    // Bonus damage from a rear attack is 1-5 plus a quarter of agility.
    private int BonusRearDamage()
    {
        return rng.Next(BonusRearDamageMin(), BonusRearDamageMax()); 
    }

    // Returns true if floats are within 15.0f of each other.
    // compare Mathf.Approximately, this has a much larger tolerance window.
    private bool VeryApproximateMatch(float f1, float f2)
    {
        if (Mathf.Abs(f1 - f2) < 15.0f) return true;
        return false;
    }

    // Returns true if the target is being attacked from the rear, left, or right.
    // Side/rear attacks double the chance of a critical hit.
    public bool IsFlanking(ObjectStats target)
    {
        float rotation1 = target.transform.eulerAngles.y;
        float rotation2 = transform.eulerAngles.y;
        return !VeryApproximateMatch(Mathf.Abs(rotation1 - rotation2), 180.0f);
    }

    // Returns true if the target is being attacked from the rear.
    // Doesn't include attacks from the left or right.
    private bool IsBackstab(ObjectStats target)
    {
        float rotation1 = target.transform.eulerAngles.y;
        float rotation2 = transform.eulerAngles.y;
        return VeryApproximateMatch(Mathf.Abs(rotation1 - rotation2), 0.0f) || VeryApproximateMatch(Mathf.Abs(rotation1 - rotation2), 360.0f);
    }

    public int CritChance()
    {
        return GetEffectiveIntelligence() / 5;
    }

    private bool IsCrit(ObjectStats target)
    {
        int chance = CritChance();
        if (IsFlanking(target))
        {
            chance *= 2;
        }
        return PercentRoll(chance);
    }

    public void PerformAttack(ObjectStats target)
    {
        Animate("IsAttacking");
        if (!PercentRoll(HitChance())) {
            target.Animate("IsDodging");
            DisplayPopup("Miss!");
            DisplayPopup("Miss");
            return;
        }
        if (PercentRoll(target.DodgeChance()))
        {
            target.Animate("IsDodging");
            target.DisplayPopup("Dodge");
            return;
        }
        int dam = DamageInflicted();
        bool backstab = false;
        if (IsBackstab(target))
        {
            backstab = true;
            dam += BonusRearDamage();
        }
        // Critical hits apply a +50% multiplier, after all other modifiers are considered.
        if (IsCrit(target))
        {
            dam += (dam / 2);
            DisplayPopup("Crit");
        }
        else if (backstab) // Only display the backstab popup if it's not a crit.
        {
            DisplayPopup("Backstab");
        }
        target.DisplayPopup(dam + " damage");
        target.ReceiveDamage(dam);
    }
}
