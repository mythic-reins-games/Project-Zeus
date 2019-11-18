using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// As ranged attack units, medusas inflict reduced damage with their base attacks.
public class MedusaMechanics : CreatureMechanics
{
    override public int MaxDamage()
    {
        return 5 + GetDamageModifiers() / 2 + GetEffectiveStrength() * 2;
    }

    override public int MinDamage()
    {
        return 1 + GetDamageModifiers() / 2 + GetEffectiveStrength() * 2;
    }

    override public int BonusRearDamageMin()
    {
        return 1 + GetEffectiveAgility();
    }

    override public int BonusRearDamageMax()
    {
        return 2 + GetEffectiveAgility();
    }
}
