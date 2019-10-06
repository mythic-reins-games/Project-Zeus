using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CombatController
{

    override protected bool CanAttack(Tile tile)
    {
        return tile.occupant.GetComponent<PlayerController>() != null;
    }

    // Update is called once per frame
    void Update()
    {
        // For now, the AI will just skip its turns.
        if (!isTurn)
        {
            return;
        }
        EndTurn();
    }
}
