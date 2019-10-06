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

    // Start is called before the first frame update
    void Start()
    {
        healthBarScript = healthBar.GetComponent<HealthBar>();
    }

    public void ReceiveDamage(int amount)
    {
        healthBarScript.TakeDamage();
    }

    public void PerformAttack(CreatureStats target)
    {
        target.ReceiveDamage(0);
    }
}
