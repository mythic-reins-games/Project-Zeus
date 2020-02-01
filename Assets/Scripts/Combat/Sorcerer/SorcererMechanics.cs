using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// As reach attack units, Myrmadons inflict slightly less damage.
public class SorcererMechanics : CreatureMechanics
{
    override protected string attackSound { get { return "Sound/Mage/Mage_Attack/Mage_Attack_Miss_1_1"; } }
    override protected string damagedSound { get { return "Sound/Mage/Mage_Damage/Mage_Received_Damage_Non_Fatal_1_1"; } }
    override protected string dieSound { get { return "Sound/Mage/Mage_Damage/Mage_Received_Damage_Fatal_1_1"; } }
    override public string footstepSound
    {
        get
        {
            switch (rng.Next(1, 5))
            {
                case 1:
                    return "Sound/Mage/Mage_Footstep/Mage_Footstep_Arena_1_1";
                    break;
                case 2:
                    return "Sound/Mage/Mage_Footstep/Mage_Footstep_Arena_1_2";
                    break;
                case 3:
                    return "Sound/Mage/Mage_Footstep/Mage_Footstep_Arena_1_3";
                    break;
                case 4:
                    return "Sound/Mage/Mage_Footstep/Mage_Footstep_Arena_1_4";
                    break;
                case 5:
                    return "Sound/Mage/Mage_Footstep/Mage_Footstep_Arena_1_5";
                    break;
                default:
                    return "";
            }
        }
    }

    override public int MaxDamage()
    {
        return 8 + GetDamageModifiers() + GetEffectiveStrength() * 4;
    }

    override public int MinDamage()
    {
        return 2 + GetDamageModifiers() + GetEffectiveStrength() * 4;
    }

    override public int BonusRearDamageMin()
    {
        return 1 + GetBackstabModifiers() + GetEffectiveAgility() * 2;
    }

    override public int BonusRearDamageMax()
    {
        return 5 + GetBackstabModifiers() + GetEffectiveAgility() * 2;
    }
}
