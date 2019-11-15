using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSlaughter : ActionBasicAttack
{

    override public int CONCENTRATION_COST { get { return 4; } }
    override public int MIN_AP_COST { get { return 4; } }
    override public TargetType TARGET_TYPE { get { return TargetType.MELEE; } }
    override protected float ATTACK_DURATION { get { return 2.0f; } }

    IEnumerator AttackAfterDelay(float time, ObjectMechanics targetMechanics)
    {
        yield return new WaitForSeconds(time);
        if (targetMechanics != null) // If the target wasn't destroyed by the first attack.
        {
            mechanics.PerformBasicAttack(targetMechanics, false);
        }
        yield break;
    }

    override protected void AttackEffects(ObjectMechanics targetMechanics)
    {
        mechanics.PerformBasicAttack(targetMechanics, false);
        StartCoroutine(AttackAfterDelay(1.0f, targetMechanics));
    }
}
