using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is a base class designed to be inherited from by Charge type actions.
public class ActionCharge : ActionBasicAttack
{
    override public TargetType TARGET_TYPE { get { return TargetType.CHARGE; } }

    override public void BeginAction(Tile targetTile)
    {
        freeMoves = Constants.ATTACK_AP_COST; // Charge actions should all get this many free moves.
        base.BeginAction(targetTile);
    }
}
