using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Gives a unit EMPOWER status effect
public class ActionBloodlust : ActionBasicAttack
{
    override public int COOLDOWN { get { return 3; } }
    override public int CONCENTRATION_COST { get { return 8; } }
    override public int MIN_AP_COST { get { return Constants.QUICK_AP_COST; } }
    override public TargetType TARGET_TYPE { get { return TargetType.ALLY_BUFF; } }

    override public string DisplayName()
    {
        return "Bloodlust";
    }

    override protected void AttackEffects(ObjectMechanics targetMechanics)
    {
        new StatusEffect(StatusEffect.EffectType.EMPOWER, 2, targetMechanics, -1);
    }
}
