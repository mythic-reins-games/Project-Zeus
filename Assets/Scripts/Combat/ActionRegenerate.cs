using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionRegenerate : Action
{

    public int FIXED_COST = 2;

    private IEnumerator WaitForRegenerationAnimations(float fDuration)
    {
        float elapsed = 0f;
        while (elapsed < fDuration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        currentPhase = phase.NONE;
        EndAction();
        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inProgress)
        {
            return;
        }
        if (currentPhase == phase.NONE)
        {
            mechanics.Animate("IsCastingSpell");
            spentActionPoints += FIXED_COST;
            currentPhase = phase.CASTING;
            new StatusEffect(
                (int)StatusEffect.EffectType.REGENERATION,
                2,
                mechanics
            );
            StartCoroutine(WaitForRegenerationAnimations(1.0f));
        }
    }

}
