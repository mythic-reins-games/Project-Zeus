using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCausticPowder : ActionBasicAttack
{
    override public int COOLDOWN { get { return 4; } }
    override public int CONCENTRATION_COST { get { return 3; } }
    override public int MIN_AP_COST { get { return Constants.ATTACK_AP_COST; } }
    override public TargetType TARGET_TYPE { get { return TargetType.REACH; } }

    override public string DisplayName()
    {
        return "Caustic Powder";
    }

    override protected void AttackEffects(ObjectMechanics targetMechanics)
    {
        mechanics.PerformAttackWithStatusEffect(targetMechanics, StatusEffect.EffectType.BLINDED, 3, -1, 0.5f);
    }
}
