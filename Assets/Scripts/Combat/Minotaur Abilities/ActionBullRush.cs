using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Charges toward a target and attacks it, possibly inflicting status effect knockdown.
public class ActionBullRush : ActionCharge
{
    override public int CONCENTRATION_COST { get { return 12; } }
    override public int MIN_AP_COST { get { return Constants.ATTACK_AP_COST; } }

    override protected void AttackEffects(ObjectMechanics targetMechanics)
    {
        mechanics.PerformAttackWithStatusEffect(targetMechanics, StatusEffect.EffectType.KNOCKDOWN, 1);
    }
}