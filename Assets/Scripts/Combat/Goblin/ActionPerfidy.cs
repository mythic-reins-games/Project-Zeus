using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Gives a target unit enhanced backstabs
public class ActionPerfidy : ActionSelfCast
{
    override public int COOLDOWN { get { return 3; } }
    override public int CONCENTRATION_COST { get { return 6; } }
    override public int MIN_AP_COST { get { return Constants.QUICK_AP_COST; } }
    override public TargetType TARGET_TYPE { get { return TargetType.SELF_ONLY; } }

    override public string DisplayName()
    {
        return "Perfidy";
    }

    override protected void ApplySelfStatusEffect()
    {
        new StatusEffect(StatusEffect.EffectType.PERFIDY, 3, mechanics, -1);
    }
}
