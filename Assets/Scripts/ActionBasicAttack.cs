using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBasicAttack : ActionMove
{

    override protected void Start()
    {
        // Save one tile at the end of the movement path:
        // this is the tile containing the target enemy.
        reserve_tiles = 1;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (!inProgress)
        {
            return;
        }
        if (phase == PHASE_MOVING)
        {
            Move();
        }
        else if (phase == PHASE_ATTACKING)
        {
            AttackPhase();
        }
        else
        {
            phase = PHASE_NONE;
            EndAction();
        }
    }

    void ResolveAttack(GameObject target)
    {
        spentActionPoints += 4;
        CreatureStats targetStats = target.GetComponent<CreatureStats>();
        GetComponent<CreatureStats>().PerformAttack(targetStats);
    }

    void AttackPhase()
    {
        if (path.Count == 1)
        {
            Tile targetTile = path.Pop();
            Vector3 direction = CalculateDirection(targetTile.transform.position);
            transform.forward = direction;
            ResolveAttack(targetTile.occupant);
        }
        else
        {
            EndAction();
        }
    }

}
