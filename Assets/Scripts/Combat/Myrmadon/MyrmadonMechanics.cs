using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// As reach attack units, Myrmadons inflict slightly less damage.
public class MyrmadonMechanics : CreatureMechanics
{
    override public int MaxDamage()
    {
        return 8 + GetDamageModifiers() + GetEffectiveStrength() * 4;
    }

    override public int MinDamage()
    {
        return 2 + GetDamageModifiers() + GetEffectiveStrength() * 4;
    }

    override public int BonusRearDamageMin()
    {
        return 1 + GetBackstabModifiers() + GetEffectiveAgility() * 2;
    }

    override public int BonusRearDamageMax()
    {
        return 5 + GetBackstabModifiers() + GetEffectiveAgility() * 2;
    }
}
