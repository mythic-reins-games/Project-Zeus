using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureStats : MonoBehaviour
{
    [SerializeField] GameObject healthBar;
    HealthBar healthBarScript;

    [SerializeField] protected int Strength = 10;
    [SerializeField] protected int Speed = 10;
    [SerializeField] protected int Endurance = 10;
    [SerializeField] protected int Agility = 10;
    [SerializeField] protected int Intelligence = 10;

    [SerializeField] protected int MaxHP = 10;

    [SerializeField] protected int CurrentHP = 10;

    // Start is called before the first frame update
    void Start()
    {
        healthBarScript = healthBar.GetComponent<HealthBar>();
    }

    public void ReceiveDamage(int amount)
    {
        CurrentHP -= amount;
        float percent = (float)CurrentHP / (float)MaxHP;
        healthBarScript.SetLifePercent(percent);
    }

    public void PerformAttack(CreatureStats target)
    {
        // For now, just do four damage.
        target.ReceiveDamage(4);
    }
}
