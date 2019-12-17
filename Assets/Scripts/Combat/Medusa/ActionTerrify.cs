using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTerrify : ActionBasicAttack
{
    override public int COOLDOWN { get { return 3; } }
    override public int CONCENTRATION_COST { get { return 8; } }
    override public int MIN_AP_COST { get { return Constants.QUICK_AP_COST; } }
    override public TargetType TARGET_TYPE { get { return TargetType.RANGED; } }

    override public string DisplayName()
    {
        return "Terrify";
    }

    override protected void AttackEffects(ObjectMechanics targetMechanics)
    {
        new StatusEffect(StatusEffect.EffectType.BLINDED, 2, targetMechanics, -1);
        targetMechanics.DisplayPopup("Blinded");
    }
}
