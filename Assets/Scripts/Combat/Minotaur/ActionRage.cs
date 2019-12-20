using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Self-buff to increase damage.
public class ActionRage : ActionSelfCast
{
    override public int COOLDOWN { get { return 3; } }
    override public int CONCENTRATION_COST { get { return 8; } }
    override public int MIN_AP_COST { get { return Constants.QUICK_AP_COST; } }

    override public string DisplayName()
    {
        return "Rage";
    }

    override protected void ApplySelfStatusEffect()
    {
        new StatusEffect(
            StatusEffect.EffectType.RAGE,
            2,
            mechanics
        );
    }
}
