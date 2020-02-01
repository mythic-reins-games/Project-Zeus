using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurMechanics : CreatureMechanics
{
    override protected string attackSound { get { return "Sound/Minotaur/Minotaur_Attack/Minotaur_Attack_Miss_1_1"; } }
    override protected string damagedSound { get { return "Sound/Minotaur/Minotaur_Damage/Minotaur_Received_Damage_Non_Fatal_1_1"; } }
    override protected string dieSound
    {
        get
        {
            switch (rng.Next(1, 2))
            {
                case 1:
                    return "Sound/Minotaur/Minotaur_Damage/Minotaur_Received_Damage_Fatal_1_1";
                case 2:
                    return "Sound/Minotaur/Minotaur_Damage/Minotaur_Received_Damage_Fatal_1_2";
                default:
                    return "";
            }
        }
    }

    override public string footstepSound
    {
        get
        {
            switch (rng.Next(1, 16))
            {
                case 1:
                    return "Sound/Minotaur/Minotaur_Footstep/Minotaur_Footstep_Arena_1_1";
                case 2:
                    return "Sound/Minotaur/Minotaur_Footstep/Minotaur_Footstep_Arena_1_2";
                case 3:
                    return "Sound/Minotaur/Minotaur_Footstep/Minotaur_Footstep_Arena_1_3";
                case 4:
                    return "Sound/Minotaur/Minotaur_Footstep/Minotaur_Footstep_Arena_1_4";
                case 5:
                    return "Sound/Minotaur/Minotaur_Footstep/Minotaur_Footstep_Arena_1_5";
                case 6:
                    return "Sound/Minotaur/Minotaur_Footstep/Minotaur_Footstep_Arena_1_6";
                case 7:
                    return "Sound/Minotaur/Minotaur_Footstep/Minotaur_Footstep_Arena_1_7";
                case 8:
                    return "Sound/Minotaur/Minotaur_Footstep/Minotaur_Footstep_Arena_1_8";
                case 9:
                    return "Sound/Minotaur/Minotaur_Footstep/Minotaur_Footstep_Arena_1_9";
                case 10:
                    return "Sound/Minotaur/Minotaur_Footstep/Minotaur_Footstep_Arena_1_10";
                case 11:
                    return "Sound/Minotaur/Minotaur_Footstep/Minotaur_Footstep_Arena_1_11";
                case 12:
                    return "Sound/Minotaur/Minotaur_Footstep/Minotaur_Footstep_Arena_1_12";
                case 13:
                    return "Sound/Minotaur/Minotaur_Footstep/Minotaur_Footstep_Arena_1_13";
                case 14:
                    return "Sound/Minotaur/Minotaur_Footstep/Minotaur_Footstep_Arena_1_14";
                case 15:
                    return "Sound/Minotaur/Minotaur_Footstep/Minotaur_Footstep_Arena_1_15";
                default:
                    return "";
            }
        }
    }
}
