using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMobility : ActionSelfCast
{
    override public int COOLDOWN { get { return 6; } }
    override public int CONCENTRATION_COST { get { return 10; } }
    override public int MIN_AP_COST { get { return Constants.QUICK_AP_COST; } }
    override public TargetType TARGET_TYPE { get { return TargetType.SELF_ONLY; } }

    override public string DisplayName()
    {
        return "Mobility";
    }

    override protected void ApplySelfStatusEffect()
    {
        new StatusEffect(StatusEffect.EffectType.MOBILITY, 5, mechanics, -1);
    }
}
