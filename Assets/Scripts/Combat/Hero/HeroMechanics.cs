using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMechanics : CreatureMechanics
{
    override protected string attackSound { get { return "Sound/Hero/Hero_Attack/Hero_Attack_Miss_1_1"; } }
    override protected string damagedSound { get { return "Sound/Hero/Hero_Damage/Hero_Received_Damage_Non_Fatal_1_2"; } }
    override protected string dieSound { get { return "Sound/Hero/Hero_Damage/Hero_Received_Damage_Non_Fatal_1_1"; } }
    override public string footstepSound { get {
        switch(rng.Next(1, 5))
            {
                case 1:
                    return "Sound/Hero/Hero_Footstep/Hero_Footstep_Arena_1_1";
                    break;
                case 2:
                    return "Sound/Hero/Hero_Footstep/Hero_Footstep_Arena_1_2";
                    break;
                case 3:
                    return "Sound/Hero/Hero_Footstep/Hero_Footstep_Arena_1_3";
                    break;
                case 4:
                    return "Sound/Hero/Hero_Footstep/Hero_Footstep_Arena_1_4";
                    break;
                case 5:
                    return "Sound/Hero/Hero_Footstep/Hero_Footstep_Arena_1_5";
                    break;
                default:
                    return "";
            }
    } }
}
