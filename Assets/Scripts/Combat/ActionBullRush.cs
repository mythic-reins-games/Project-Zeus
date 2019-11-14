using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBullRush : ActionBasicAttack
{

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
