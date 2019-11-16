using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Double-attack melee attack.
public class ActionSlaughter : ActionBasicAttack
{
    override public int COOLDOWN { get { return 2; } }
    override public int CONCENTRATION_COST { get { return 4; } }
    override public int MIN_AP_COST { get { return Constants.ATTACK_AP_COST; } }
    override public TargetType TARGET_TYPE { get { return TargetType.MELEE; } }

    override protected void AttackEffects(ObjectMechanics targetMechanics)
    {
        mechanics.PerformBasicAttack(targetMechanics, false, 1.5f);
    }
}
