using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Gives a unit EMPOWER status effect
public class ActionEmpower : ActionSelfCast
{
    override public int COOLDOWN { get { return 4; } }
    override public int CONCENTRATION_COST { get { return 6; } }
    override public int MIN_AP_COST { get { return Constants.QUICK_AP_COST; } }

    override public string DisplayName()
    {
        return "Empower";
    }

    override protected void ApplySelfStatusEffect()
    {
        new StatusEffect(
            StatusEffect.EffectType.EMPOWER,
            2,
            mechanics
        );
    }
}
