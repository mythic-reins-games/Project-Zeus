using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CreatureStats : MonoBehaviour
{

    System.Random rng;

    [SerializeField] GameObject healthBar;
    IndicatorBar healthBarScript;

    [SerializeField] GameObject staminaBar;
    IndicatorBar staminaBarScript;

    [SerializeField] private int Strength = 10;
    [SerializeField] private int Speed = 10;
    [SerializeField] private int Endurance = 10;
    [SerializeField] private int Agility = 10;
    [SerializeField] private int Intelligence = 10;

    protected int MaxHealth = 1;
    protected int CurrentHealth = 1;

    protected int MaxStamina = 1;
    protected int CurrentStamina = 1;

    float textLabelTimer = 0f;
    string textLabelText = "";

    void OnGUI()
    {
        if (textLabelTimer <= 0f) return;
        textLabelTimer -= Time.deltaTime;
        GUILayout.Label(textLabelText);
    }

    // Start is called before the first frame update
    void Start()
    {
        rng = new System.Random();
        healthBarScript = healthBar.GetComponent<IndicatorBar>();
        staminaBarScript = staminaBar.GetComponent<IndicatorBar>();
        MaxHealth = Endurance * 3 + Strength;
        MaxStamina = Endurance * 2;
        CurrentStamina = MaxStamina;
        CurrentHealth = MaxHealth;
    }

    // Average of strength and strength times lifepercent
    protected int GetEffectiveStrength()
    {
        float[] ar = new float[2] { Strength, Strength * PercentHealth() };
        return Mathf.RoundToInt(ar.Average());
    }

    // Average of agility and agility times lifepercent
    protected int GetEffectiveAgility()
    {
        float[] ar = new float[2] { Agility, Agility * PercentHealth() };
        return Mathf.RoundToInt(ar.Average());
    }

    // Average of intelligence and intelligence times lifepercent
    protected int GetEffectiveIntelligence()
    {
        float[] ar = new float[2] { Intelligence, Intelligence * PercentHealth() };
        return Mathf.RoundToInt(ar.Average());
    }

    // Average of Speed and Speed times lifepercent
    protected int GetEffectiveSpeed()
    {
        float[] ar = new float[2] { Speed, Speed * PercentHealth() };
        return Mathf.RoundToInt(ar.Average());
    }

    private float PercentHealth()
    {
        return (float)CurrentHealth / (float)MaxHealth;
    }

    private float PercentStamina()
    {
        return (float)CurrentStamina / (float)MaxStamina;
    }

    public int GetMaxActionPoints()
    {
        return 5 + GetEffectiveSpeed() / 10;
    }

    public void ReceiveDamage(int amount)
    {
        if (CurrentStamina >= amount)
        {
            CurrentStamina -= amount;
        }
        else
        {
            CurrentHealth -= (amount - CurrentStamina);
            CurrentStamina = 0;
        }
        healthBarScript.SetPercent(PercentHealth());
        staminaBarScript.SetPercent(PercentStamina());
    }

    private int HitChance()
    {
        return 75 + GetEffectiveAgility() / 4;
    }

    private int DodgeChance()
    {
        return 0 + GetEffectiveAgility() / 2;
    }

    private bool PercentRoll(int percent)
    {
        return rng.Next(0, 99) < percent;
    }

    // Damage from a basic attack is 1-11 plus half of strength.
    private int DamageInflicted()
    {
        return rng.Next(1, 11) + GetEffectiveStrength() / 2;
    }

    // Bonus damage from a rear attack is 1-5 plus a quarter of agility.
    private int BonusRearDamage()
    {
        return rng.Next(1, 5) + GetEffectiveAgility() / 4;
    }

    private void DisplayPopup(string text)
    {
        textLabelTimer = 5.0f;
        textLabelText = text;
    }

    // Returns true if floats are within 15.0f of each other.
    // compare Mathf.Approximately.
    private bool VeryApproximately(float f1, float f2)
    {
        if (Mathf.Abs(f1 - f2) < 15.0f) return true;
        return false;
    }

    // Returns true if the target is being attacked from the side or rear.
    // Side/read attacks double the chance of a critical hit.
    public bool IsFlanking(CreatureStats target)
    {
        float rotation1 = target.transform.eulerAngles.y;
        float rotation2 = transform.eulerAngles.y;
        bool flanking = !VeryApproximately(Mathf.Abs(rotation1 - rotation2), 180.0f);
        return flanking;
    }

    // Returns true if the target is being attacked from the rear.
    // Rear attacks do a bonus 0-10 damage plus one half of agility.
    public bool IsBehind(CreatureStats target)
    {
        float rotation1 = target.transform.eulerAngles.y;
        float rotation2 = transform.eulerAngles.y;
        bool behind = VeryApproximately(Mathf.Abs(rotation1 - rotation2), 0.0f) || VeryApproximately(Mathf.Abs(rotation1 - rotation2), 360.0f);
        return behind;
    }

    public bool IsCrit(CreatureStats target)
    {
        int chance = GetEffectiveIntelligence() / 10;
        if (IsFlanking(target))
        {
            chance *= 2;
        }
        return PercentRoll(chance);
    }

    public void PerformAttack(CreatureStats target)
    {
        if (!PercentRoll(HitChance())) {
            DisplayPopup("Miss!");
            return;
        }
        if (PercentRoll(target.DodgeChance()))
        {
            target.DisplayPopup("Dodge!");
            return;
        }
        int dam = DamageInflicted();
        if (IsBehind(target))
        {
            DisplayPopup("Rear attack!");
            dam += BonusRearDamage();
        }
        if (IsCrit(target))
        {
            target.DisplayPopup("CRITICAL HIT! " + dam + " damage inflicted!");
            dam += (dam / 2);
        }
        else
        {
            target.DisplayPopup(dam + " damage inflicted!");
        }
        target.ReceiveDamage(dam);
    }
}
