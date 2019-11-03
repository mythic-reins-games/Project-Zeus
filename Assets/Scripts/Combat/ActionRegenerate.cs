using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionRegenerate : Action
{

    protected const int PHASE_NONE = 0;
    protected const int PHASE_ANIMATING = 1;

    protected int phase = PHASE_NONE;

    private IEnumerator WaitForRegenerationAnimations(float fDuration)
    {
        float elapsed = 0f;
        while (elapsed < fDuration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        phase = PHASE_NONE;
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
        if (phase == PHASE_NONE)
        {
            spentActionPoints += 2;
            phase = PHASE_ANIMATING;
            new StatusEffect(
                (int)StatusEffect.EffectType.EFFECT_REGENERATION,
                2,
                GetComponent<CreatureStats>()
            );
            StartCoroutine(WaitForRegenerationAnimations(1.0f));
        }
    }

}
