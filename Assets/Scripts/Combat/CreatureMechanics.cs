#pragma warning disable 0649

using UnityEngine;
using System.Linq;
using System.Collections.Generic;

// This class implements the high-level game rules for creatures.
public class CreatureMechanics : ObjectMechanics
{
    System.Random rng;

    IndicatorBar staminaBarScript;
    IndicatorBar healthBarScript;

    [SerializeField] private int strength;
    [SerializeField] private int speed;
    [SerializeField] private int endurance;
    [SerializeField] private int agility;
    [SerializeField] private int intelligence;
    [SerializeField] private int maxConcentration;

    [SerializeField] public string displayName;

    protected int maxStamina;
    protected int currentStamina;
    public int currentConcentration = 0;

    private bool firstBlood = false;

    public int Speed => speed;

    override public bool canBeBackstabbed { get { return true; } }

    public void Init(int inputMaxHealth, int inputCurrentHealth, int spe, int end, int str, int agi, int intel, string name)
    {
        displayName = name;
        speed = spe;
        endurance = end;
        strength = str;
        agility = agi;
        intelligence = intel;
        rng = new System.Random();
        healthBarScript = transform.Find("HealthBar").GetComponent<IndicatorBar>();
        staminaBarScript = transform.Find("StaminaBar").GetComponent<IndicatorBar>();
        maxHealth = inputMaxHealth;
        maxStamina = endurance * 15 + 10;
        currentStamina = maxStamina;
        currentHealth = inputCurrentHealth;
        maxConcentration = GetEffectiveIntelligence() * 10;
    }

    public void RegisterStatusEffect(StatusEffect effect)
    {
        statusEffects.Add(effect);
        SetStatusAnim(effect.GetAnimationName());
    }

    public int BeginTurnAndGetMaxActionPoints()
    {
        int ap = 5 + GetEffectiveSpeed();
        foreach (StatusEffect effect in statusEffects)
        {
            ap = effect.PerRoundEffect(ap);
            // The PerRoundEffect may have killed the unit (poison, burning).
            if (dead) return 0;
        }
        statusEffects.RemoveAll(e => e.expired);
        return ap;
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

    // Poison damages health but not stamina.
    public void ReceivePureDamage(int amount)
    {
        amount = (int)((float)amount * DefensiveDamageMultiplier());
        DisplayPopup(amount + " poison");
        if (!firstBlood)
        {
            firstBlood = true;
            BoostConcentration();
        }
        currentHealth -= amount;
        if (currentHealth <= 0 && StatusEffect.HasEffectType(ref statusEffects, StatusEffect.EffectType.CANNOT_DIE))
            currentHealth = 1;
    }

    // Healing heals health but not stamina.
    public void ReceiveHealing(int amount)
    {
        DisplayPopup(amount + " healing");
        if (currentHealth + amount > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth += amount;
        }
        healthBarScript.SetPercent(PercentHealth());
    }

    public float GetConcentrationPercent()
    {
        return (float)currentConcentration / (float)maxConcentration;
    }

    override public void ReceiveDamage(int amount)
    {
        amount = (int)((float)amount * DefensiveDamageMultiplier());
        DisplayPopupAfterDelay(0.2f, amount + " damage");
        if (currentStamina >= amount)
        {
            currentStamina -= amount;
        }
        else
        {
            if (!firstBlood)
            {
                firstBlood = true;
                BoostConcentration();
            }
            currentHealth -= (amount - currentStamina);
            if (currentHealth <= 0 && StatusEffect.HasEffectType(ref statusEffects, StatusEffect.EffectType.CANNOT_DIE))
                currentHealth = 1;
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

    protected int GetDodgeModifiers()
    {
        int m = 0;
        if (StatusEffect.HasEffectType(ref statusEffects, StatusEffect.EffectType.EMPOWER))
            m += 20;
        if (StatusEffect.HasEffectType(ref statusEffects, StatusEffect.EffectType.BLINDED))
            m -= 30;
        return m;
    }

    protected int GetHitModifiers()
    {
        int m = 0;
        if (StatusEffect.HasEffectType(ref statusEffects, StatusEffect.EffectType.BLINDED))
            m -= 30;
        return m;
    }

    public int HitChance()
    {
        return 80 + GetHitModifiers() + (GetEffectiveAgility() * 2);
    }

    override public int DodgeChance()
    {
        return GetDodgeModifiers() + (GetEffectiveAgility() * 5);
    }

    private bool PercentRoll(int percent)
    {
        return rng.Next(0, 99) < percent;
    }

    protected int GetDamageModifiers()
    {
        int m = 0;
        if (StatusEffect.HasEffectType(ref statusEffects, StatusEffect.EffectType.RAGE))
            m += 10;
        if (StatusEffect.HasEffectType(ref statusEffects, StatusEffect.EffectType.EMPOWER))
            m += 5;
        return m;
    }

    virtual public int MaxDamage()
    {
        return 10 + GetDamageModifiers() + GetEffectiveStrength() * 4;
    }

    virtual public int MinDamage()
    {
        return 2 + GetDamageModifiers() + GetEffectiveStrength() * 4;
    }

    // Damage from a basic attack is 1-11 plus half of strength.
    private int DamageInflicted()
    {
        return rng.Next(MinDamage(), MaxDamage());
    }

    virtual public int BonusRearDamageMin()
    {
        return 2 + GetEffectiveAgility() * 2;
    }

    virtual public int BonusRearDamageMax()
    {
        return 4 + GetEffectiveAgility() * 2;
    }

    private float DefensiveDamageMultiplier()
    {
        float m = 1.0f;
        if (StatusEffect.HasEffectType(ref statusEffects, StatusEffect.EffectType.PETRIFIED)) m /= 2.0f;
        return m;
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

    // Returns true if the target is being attacked from the rear.
    // Doesn't include attacks from the left or right.
    private bool IsBackstab(ObjectMechanics target)
    {
        float rotation1 = target.transform.eulerAngles.y;
        float rotation2 = transform.eulerAngles.y;
        return VeryApproximateMatch(Mathf.Abs(rotation1 - rotation2), 0.0f) || VeryApproximateMatch(Mathf.Abs(rotation1 - rotation2), 360.0f);
    }

    private bool IsVulnerable(ObjectMechanics target)
    {
        if (StatusEffect.HasEffectType(ref target.statusEffects, StatusEffect.EffectType.PETRIFIED)) return false;
        if (!target.canBeBackstabbed) return false;
        if (IsBackstab(target)) return true;
        if (StatusEffect.HasEffectType(ref target.statusEffects, StatusEffect.EffectType.KNOCKDOWN)) return true;
        if (StatusEffect.HasEffectType(ref target.statusEffects, StatusEffect.EffectType.BLINDED)) return true;
        return false;
    }

    override protected void Die()
    {
        base.Die();
    }

    public int CritChance()
    {
        return GetEffectiveIntelligence() * 5;
    }

    private bool IsCrit(ObjectMechanics target)
    {
        int chance = CritChance();
        if (IsVulnerable(target))
        {
            chance *= 2;
        }
        return PercentRoll(chance);
    }

    public void PerformAttackWithStatusEffect(ObjectMechanics target, StatusEffect.EffectType type, int duration, int powerLevel = -1, float damageMultiplier = 1.0f)
    {
        if(HitAndDamage(target, false, damageMultiplier))
        {
            new StatusEffect(type, duration, target, powerLevel);
        }
    }

    // Public wrapper for HitAndDamage
    public void PerformBasicAttack(ObjectMechanics target, bool isConcentrationEligible = true, float damageMultiplier = 1.0f)
    {
        HitAndDamage(target, isConcentrationEligible, damageMultiplier);
    }

    private void BoostConcentration()
    {
        currentConcentration += GetEffectiveIntelligence() * 2;
    }

    private float HeightAdvantageMultiplier(ObjectMechanics target)
    {
        float relativeHeight = transform.position.y - target.transform.position.y;
        if (relativeHeight > 0.25f) return 1.2f;
        if (relativeHeight > 0f) return 1.1f;
        return 1.0f;
    }

    private bool HitAndDamage(ObjectMechanics target, bool isConcentrationEligible, float damageMultiplier)
    {
        if (isConcentrationEligible)
        {
            BoostConcentration();
        }
        Animate("IsAttacking");
        if (!PercentRoll(HitChance())) {
            target.Animate("IsDodging");
            DisplayPopupAfterDelay(0.2f, "Miss");
            return false;
        }
        if (PercentRoll(target.DodgeChance()))
        {
            target.Animate("IsDodging");
            target.DisplayPopupAfterDelay(0.2f, "Dodge");
            return false;
        }
        int dam = DamageInflicted();
        bool backstab = false;
        if (IsVulnerable(target))
        {
            if (isConcentrationEligible) {
                BoostConcentration();
            }
            backstab = true;
            dam += BonusRearDamage();
        }
        // Critical hits apply a +100% multiplier, after all other modifiers are considered.
        if (IsCrit(target))
        {
            dam += dam;
            DisplayPopupAfterDelay(0.2f, "Crit");
        }
        else if (backstab) // Only display the backstab popup if it's not a crit.
        {
            DisplayPopupAfterDelay(0.2f, "Backstab");
        }
        damageMultiplier *= HeightAdvantageMultiplier(target);
        dam = (int)((float)dam * damageMultiplier);
        target.ReceiveDamage(dam);
        return true;
    }
}
