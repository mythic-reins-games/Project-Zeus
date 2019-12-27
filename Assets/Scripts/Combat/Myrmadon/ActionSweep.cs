using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSweep : ActionBasicAttack
{
    override public int COOLDOWN { get { return 4; } }
    override public int CONCENTRATION_COST { get { return 8; } }
    override public int MIN_AP_COST { get { return Constants.ATTACK_AP_COST; } }
    override public TargetType TARGET_TYPE { get { return TargetType.REACH; } }

    override public string DisplayName()
    {
        return "Sweep";
    }

    override protected void AttackEffects(ObjectMechanics targetMechanics)
    {
        mechanics.PerformAttackWithStatusEffect(targetMechanics, StatusEffect.EffectType.KNOCKDOWN, 1, -1, 1f);
    }
}
