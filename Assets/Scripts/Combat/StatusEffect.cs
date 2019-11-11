﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect
{
    public enum EffectType
    {
        REGENERATION,
        KNOCKDOWN
    };

    const string LYING_DOWN = "IsLyingDown";

    EffectType type;
    int roundsRemaining = 0;
    CreatureStats target;
    int powerLevel;
    public bool expired = false;

    public string GetAnimationName()
    {
        switch (type)
        {
            case EffectType.REGENERATION:
                return null;
            case EffectType.KNOCKDOWN:
                return LYING_DOWN;
        }
        return null;
    }

    // Some status effects have varying power levels, others default to -1
    public StatusEffect(EffectType effectType, int durationRounds, ObjectStats effectTarget, int effectPowerLevel = -1)
    {
        // Only CreatureStats can get status effects.
        if (effectTarget.GetType() == typeof(ObjectStats))
        {
            return;
        }
        roundsRemaining = durationRounds;
        type = effectType;
        target = (CreatureStats)effectTarget;
        powerLevel = effectPowerLevel;
        target.RegisterStatusEffect(this);
    }

    // The CreatureStats is responsible for calling this every round before its action.
    // It can change the creature's action points.
    public int PerRoundEffect(int ap)
    {
        switch (type)
        {
            case EffectType.REGENERATION:
                target.ReceiveHealing(5);
                break;
            case EffectType.KNOCKDOWN:
                ap = 0;
                break;
        }
        roundsRemaining--;
        if (roundsRemaining == 0)
        {
            target.UnsetStatusAnimation(GetAnimationName());
            expired = true;
        }
        return ap;
    }
}