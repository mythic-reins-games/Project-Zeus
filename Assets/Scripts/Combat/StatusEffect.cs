using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect
{
    public enum EffectType
    {
        EFFECT_REGENERATION,
    };

    EffectType type;
    int roundsRemaining = 0;
    CreatureStats target;
    int powerLevel;
    public bool expired = false;

    // Some status effects have varying power levels, others default to -1
    public StatusEffect(EffectType effectType, int durationRounds, CreatureStats effectTarget, int effectPowerLevel = -1)
    {
        roundsRemaining = durationRounds;
        type = effectType;
        target = effectTarget;
        powerLevel = effectPowerLevel;
        target.RegisterStatusEffect(this);
    }

    // The CreatureStats is responsible for calling this every round before its action.
    public void PerRoundEffect()
    {
        switch (type)
        {
            case EffectType.EFFECT_REGENERATION:
                target.ReceiveHealing(5);
                break;
        }
        roundsRemaining--;
        if (roundsRemaining == 0)
        {
            expired = true;
        }
    }
}
