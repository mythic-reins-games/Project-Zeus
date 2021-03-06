﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Status effect that will automatically apply itself to a target creature upon instantiation.
// Can buff or debuff the target.
public class StatusEffect
{
    public enum EffectType
    {
        REGENERATION,
        KNOCKDOWN,
        RAGE,
        CANNOT_DIE,
        EMPOWER,
        BLINDED,
        PETRIFIED,
        POISONED,
        FROZEN,
        BURNING,
        PERFIDY,
        MOBILITY,
        BULWARK,
        SLOWED,
    };

    const string LYING_DOWN = "IsLyingDown";

    EffectType type;
    int roundsRemaining = 0;
    CreatureMechanics target;
    int powerLevel;
    public bool expired = false;

    static public bool HasEffectType(ref List<StatusEffect> check, EffectType effectType)
    {
        foreach (StatusEffect effect in check)
        {
            if (effect.type == effectType) return true;
        }
        return false;
    }

    public string GetAnimationName()
    {
        switch (type)
        {
            case EffectType.KNOCKDOWN:
                return LYING_DOWN;
        }
        return null;
    }

    // Some status effects have varying power levels, others default to -1
    public StatusEffect(EffectType effectType, int durationRounds, ObjectMechanics effectTarget, int effectPowerLevel = -1)
    {
        // Only CreatureMechanics can get status effects.
        if (effectTarget.GetType() == typeof(ObjectMechanics))
        {
            return;
        }
        roundsRemaining = durationRounds;
        type = effectType;
        target = (CreatureMechanics)effectTarget;
        powerLevel = effectPowerLevel;
        target.RegisterStatusEffect(this);
    }

    // The CreatureMechanics is responsible for calling this every round before its action.
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
                target.DisplayPopup("Knockdown");
                break;
            case EffectType.PETRIFIED:
                ap = 0;
                target.DisplayPopup("Petrified");
                break;
            case EffectType.POISONED:
                target.ReceivePureDamage(5);
                break;
            case EffectType.SLOWED:
                // -4 AP, but never takes a unit below 2 AP.
                target.DisplayPopup("Slowed");
                if (ap > 6) ap -= 4;
                else if (ap > 0) ap = 2;
                break;
            case EffectType.FROZEN:
                target.DisplayPopup("Frozen");
                ap = 0;
                break;
            case EffectType.MOBILITY:
                if (ap > 0) ap += 2; // +2 AP, as long as the unit isn't disabled.
                break;
            case EffectType.BURNING:
                target.ReceiveDamage(10);
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
