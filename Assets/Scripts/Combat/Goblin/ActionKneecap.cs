using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionKneecap : ActionBasicAttack
{
    override public int COOLDOWN { get { return 3; } }
    override public int CONCENTRATION_COST { get { return 9; } }
    override public int MIN_AP_COST { get { return Constants.ATTACK_AP_COST; } }
    override public TargetType TARGET_TYPE { get { return TargetType.MELEE; } }
    override protected float ATTACK_DURATION { get { return 2.0f; } }

    override public string DisplayName()
    {
        return "Kneecap";
    }

    IEnumerator AttackAfterDelay(float time, ObjectMechanics targetMechanics)
    {
        yield return new WaitForSeconds(time);
        if (targetMechanics != null) // If the target wasn't destroyed by the first attack.
        {
            mechanics.PerformAttackWithStatusEffect(targetMechanics, StatusEffect.EffectType.SLOWED, 2, -1, 0.5f);
        }
        yield break;
    }

    override protected void AttackEffects(ObjectMechanics targetMechanics)
    {
        mechanics.PerformAttackWithStatusEffect(targetMechanics, StatusEffect.EffectType.KNOCKDOWN, 1, -1, 0.5f);
        StartCoroutine(AttackAfterDelay(1.0f, targetMechanics));
    }
}
