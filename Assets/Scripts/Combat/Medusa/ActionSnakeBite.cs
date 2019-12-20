using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSnakeBite : ActionBasicAttack
{
    override public int COOLDOWN { get { return 2; } }
    override public int CONCENTRATION_COST { get { return 6; } }
    override public int MIN_AP_COST { get { return Constants.QUICK_AP_COST; } }
    override public TargetType TARGET_TYPE { get { return TargetType.MELEE; } }

    override public string DisplayName()
    {
        return "Snake Bite";
    }

    override protected void AttackEffects(ObjectMechanics targetMechanics)
    {
        mechanics.PerformAttackWithStatusEffect(targetMechanics, StatusEffect.EffectType.POISONED, 2, -1, 0.5f);
    }
}
