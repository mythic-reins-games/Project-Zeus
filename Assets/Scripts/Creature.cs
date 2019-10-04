using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{

    [SerializeField] protected int Strength = 10;
    [SerializeField] protected int Speed = 10;
    [SerializeField] protected int Endurance = 10;
    [SerializeField] protected int Agility = 10;
    [SerializeField] protected int Intelligence = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ReceiveDamage(int amount)
    {

    }

    public void PerformAttack(GameObject target)
    {

    }
}
