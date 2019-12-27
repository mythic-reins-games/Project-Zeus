using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPetrify : ActionBasicAttack
{
    override public int COOLDOWN { get { return 5; } }
    override public int CONCENTRATION_COST { get { return 14; } }
    override public int MIN_AP_COST { get { return Constants.ATTACK_AP_COST; } }
    override public TargetType TARGET_TYPE { get { return TargetType.MELEE; } }

    override public string DisplayName()
    {
        return "Petrify";
    }

    override protected void AttackEffects(ObjectMechanics targetMechanics)
    {
        new StatusEffect(StatusEffect.EffectType.PETRIFIED, 3, targetMechanics, -1);
        targetMechanics.DisplayPopup("Petrified");
    }
}
