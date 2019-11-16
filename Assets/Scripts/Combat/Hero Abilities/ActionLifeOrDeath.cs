using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Gives a unity CANNOT_DIE status effect.
public class ActionLifeOrDeath : ActionSelfCast
{
    override public int CONCENTRATION_COST { get { return 4; } }
    override public int MIN_AP_COST { get { return Constants.QUICK_AP_COST; } }

    override protected void ApplySelfStatusEffect()
    {
        new StatusEffect(
            StatusEffect.EffectType.CANNOT_DIE,
            1,
            mechanics
        );
    }
}
