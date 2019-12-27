using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionIgnite : ActionBasicAttack
{
    override public int COOLDOWN { get { return 3; } }
    override public int CONCENTRATION_COST { get { return 10; } }
    override public int MIN_AP_COST { get { return Constants.ATTACK_AP_COST; } }
    override public TargetType TARGET_TYPE { get { return TargetType.RANGED; } }

    override public string DisplayName()
    {
        return "Ignite";
    }

    override protected void AttackEffects(ObjectMechanics targetMechanics)
    {
        new StatusEffect(StatusEffect.EffectType.BURNING, 5, targetMechanics, -1);
        targetMechanics.DisplayPopup("Ignited");
    }
}
