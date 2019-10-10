using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CreatureStats : MonoBehaviour
{

    System.Random rng;

    [SerializeField] GameObject healthBar;
    HealthBar healthBarScript;

    [SerializeField] GameObject staminaBar;
    HealthBar staminaBarScript;

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
        healthBarScript = healthBar.GetComponent<HealthBar>();
        staminaBarScript = staminaBar.GetComponent<HealthBar>();
        MaxHealth = Endurance * 3 + Strength;
        MaxStamina = Endurance * 2;
        CurrentStamina = MaxStamina;
        CurrentHealth = MaxHealth;
    }

    // Average of strength and strength times lifepercent
    protected int GetEffectiveStrength()
    {
        float[] ar = new float[2] { Strength, Strength * percentHealth() };
        return Mathf.RoundToInt(ar.Average());
    }

    // Average of agility and agility times lifepercent
    protected int GetEffectiveAgility()
    {
        float[] ar = new float[2] { Agility, Agility * percentHealth() };
        return Mathf.RoundToInt(ar.Average());
    }

    // Average of intelligence and intelligence times lifepercent
    protected int GetEffectiveIntelligence()
    {
        float[] ar = new float[2] { Intelligence, Intelligence * percentHealth() };
        return Mathf.RoundToInt(ar.Average());
    }

    // Average of Speed and Speed times lifepercent
    protected int GetEffectiveSpeed()
    {
        float[] ar = new float[2] { Speed, Speed * percentHealth() };
        return Mathf.RoundToInt(ar.Average());
    }

    private float percentHealth()
    {
        return (float)CurrentHealth / (float)MaxHealth;
    }

    private float percentStamina()
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
        healthBarScript.SetPercent(percentHealth());
        staminaBarScript.SetPercent(percentStamina());
    }

    private int hitChance()
    {
        return 75 + GetEffectiveAgility() / 4;
    }

    private int dodgeChance()
    {
        return 0 + GetEffectiveAgility() / 2;
    }

    private bool percentRoll(int percent)
    {
        return rng.Next(0, 99) < percent;
    }

    private int damageInflicted()
    {
        return rng.Next(0, 10) + GetEffectiveStrength() / 2;
    }

    private void DisplayPopup(string text)
    {
        textLabelTimer = 5.0f;
        textLabelText = text;
    }

    public void PerformAttack(CreatureStats target)
    {
        if (!percentRoll(hitChance())) {
            DisplayPopup("Miss!");
            return;
        }
        if (percentRoll(target.dodgeChance()))
        {
            target.DisplayPopup("Dodge!");
            return;
        }
        int dam = damageInflicted();
        target.DisplayPopup(dam + " damage inflicted!");
        target.ReceiveDamage(dam);
    }
}
