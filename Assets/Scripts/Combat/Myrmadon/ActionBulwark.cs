using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBulwark : ActionSelfCast
{
    override public int COOLDOWN { get { return 1; } }
    override public int CONCENTRATION_COST { get { return 3; } }
    override public int MIN_AP_COST { get { return Constants.QUICK_AP_COST; } }
    override public TargetType TARGET_TYPE { get { return TargetType.SELF_ONLY; } }

    override public string DisplayName()
    {
        return "Bulwark";
    }

    override protected void ApplySelfStatusEffect()
    {

        new StatusEffect(StatusEffect.EffectType.BULWARK, 1, mechanics, -1);
    }
}
