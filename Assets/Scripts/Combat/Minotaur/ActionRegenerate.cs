using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Self healing-over-time buff.
public class ActionRegenerate : ActionSelfCast
{
    override public int COOLDOWN { get { return 3; } }
    override public int CONCENTRATION_COST { get { return 4; } }
    override public int MIN_AP_COST { get { return Constants.QUICK_AP_COST; } }

    override public string DisplayName()
    {
        return "Regenerate";
    }

    override protected void ApplySelfStatusEffect()
    {
        new StatusEffect(
            StatusEffect.EffectType.REGENERATION,
            2,
            mechanics
        );
    }
}
