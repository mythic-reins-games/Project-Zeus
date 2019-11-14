using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBullRush : ActionBasicAttack
{

    override public int CONCENTRATION_COST { get { return 12; } }
    override public int MIN_AP_COST { get { return 4; } }
    override public TargetType TARGET_TYPE { get { return TargetType.CHARGE; } }

    override protected void AttackEffects(ObjectMechanics targetMechanics)
    {
        mechanics.PerformAttackWithStatusEffect(targetMechanics, StatusEffect.EffectType.KNOCKDOWN, 1);
    }

    override public void BeginAction(Tile targetTile)
    {
        freeMoves += 4;
        base.BeginAction(targetTile);
    }
}
