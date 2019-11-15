using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is a base class designed to be inherited from by self cast type actions.
public class ActionSelfCast : Action
{
    override public TargetType TARGET_TYPE { get { return TargetType.SELF_ONLY; } }

    virtual protected void ApplySelfStatusEffect() { }

    // Update is called once per frame
    void Update()
    {
        if (!inProgress)
        {
            return;
        }
        if (currentPhase == Phase.NONE)
        {
            mechanics.Animate("IsCastingSpell");
            spentActionPoints += MIN_AP_COST;
            currentPhase = Phase.CASTING;
            ApplySelfStatusEffect();
            StartCoroutine(EndActionAfterDelay(1.0f));
        }
    }
}
