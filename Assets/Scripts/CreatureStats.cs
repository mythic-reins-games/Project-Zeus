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

    [SerializeField] private int strength = 10;
    [SerializeField] private int speed = 10;
    [SerializeField] private int endurance = 10;
    [SerializeField] private int agility = 10;
    [SerializeField] private int intelligence = 10;

    protected int maxHealth = 1;
    protected int currentHealth = 1;

    protected int maxStamina = 1;
    protected int currentStamina = 1;

    float textLabelTimer = 0f;
    string textLabelText = "";

    private Animator anim;

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
        maxHealth = endurance * 3 + strength;
        maxStamina = endurance * 2;
        currentStamina = maxStamina;
        currentHealth = maxHealth;
        anim = GetComponentInChildren<Animator>();
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

    private float PercentHealth()
    {
        return (float)currentHealth / (float)maxHealth;
    }

    private float PercentStamina()
    {
        return (float)currentStamina / (float)maxStamina;
    }

    public int GetMaxActionPoints()
    {
        return 5 + GetEffectiveSpeed() / 10;
    }

    public void ReceiveDamage(int amount)
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
    }

    private int HitChance()
    {
        return 75 + (GetEffectiveAgility() / 4);
    }

    private int DodgeChance()
    {
        return 0 + (GetEffectiveAgility() / 2);
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
    // compare Mathf.Approximately, this has a much larger tolerance window.
    private bool VeryApproximateMatch(float f1, float f2)
    {
        if (Mathf.Abs(f1 - f2) < 15.0f) return true;
        return false;
    }

    // Returns true if the target is being attacked from the rear, left, or right.
    // Side/rear attacks double the chance of a critical hit.
    public bool IsFlanking(CreatureStats target)
    {
        float rotation1 = target.transform.eulerAngles.y;
        float rotation2 = transform.eulerAngles.y;
        return !VeryApproximateMatch(Mathf.Abs(rotation1 - rotation2), 180.0f);
    }

    // Returns true if the target is being attacked from the rear.
    public bool IsBehind(CreatureStats target)
    {
        float rotation1 = target.transform.eulerAngles.y;
        float rotation2 = transform.eulerAngles.y;
        return VeryApproximateMatch(Mathf.Abs(rotation1 - rotation2), 0.0f) || VeryApproximateMatch(Mathf.Abs(rotation1 - rotation2), 360.0f);
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

    private IEnumerator ClearAttackAnimationsAfterDelay(float fDuration)
    {
        float elapsed = 0f;
        while (elapsed < fDuration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        anim.SetBool("IsAttacking", false);
        anim.SetBool("IsDodging", false);
        anim.SetBool("IsGettingDamaged", false);
        yield break;
    }

    public void PerformAttack(CreatureStats target)
    {
        StartCoroutine(ClearAttackAnimationsAfterDelay(0.5f));
        StartCoroutine(target.ClearAttackAnimationsAfterDelay(0.5f));
        anim.SetBool("IsAttacking", true);
        if (!PercentRoll(HitChance())) {
            target.anim.SetBool("IsDodging", true);
            DisplayPopup("Miss!");
            return;
        }
        if (PercentRoll(target.DodgeChance()))
        {
            target.anim.SetBool("IsDodging", true);
            target.DisplayPopup("Dodge!");
            return;
        }
        int dam = DamageInflicted();
        if (IsBehind(target))
        {
            DisplayPopup("Rear attack!");
            dam += BonusRearDamage();
        }
        // Critical hits apply a +50% multiplier, after all other modifiers are considered.
        if (IsCrit(target))
        {
            target.DisplayPopup("CRITICAL HIT! " + dam + " damage inflicted!");
            dam += (dam / 2);
        }
        else
        {
            target.DisplayPopup(dam + " damage inflicted!");
        }
        target.anim.SetBool("IsGettingDamaged", true);
        target.ReceiveDamage(dam);
    }
}
