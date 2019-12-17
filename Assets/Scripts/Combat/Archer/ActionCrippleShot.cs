using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCrippleShot : ActionBasicAttack
{
    override public int COOLDOWN { get { return 4; } }
    override public int CONCENTRATION_COST { get { return 10; } }
    override public int MIN_AP_COST { get { return Constants.ATTACK_AP_COST; } }
    override public TargetType TARGET_TYPE { get { return TargetType.RANGED; } }

    override public string DisplayName()
    {
        return "Crippling Shot";
    }

    override protected void AttackEffects(ObjectMechanics targetMechanics)
    {
        mechanics.PerformAttackWithStatusEffect(targetMechanics, StatusEffect.EffectType.SLOWED, 3);
    }
}
