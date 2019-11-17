using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// EnemyControllers handle the AI decision-making for creatures on the enemy side.
public class EnemyController : CombatController
{

    override protected bool ContainsEnemy(Tile tile)
    {
        if (tile.occupant == null) return false;
        return tile.HasPC();
    }

    override public bool IsStaticBlocker()
    {
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        // For now, the AI will just skip its turns.
        if (!isTurn)
        {
            return;
        }
        if (!isActing)
        {
            Tile choice = AIChooseMove();
            if (choice == null)
            {
                EndTurn();
            }
            else
            {
                if (choice.occupant != null)
                {
                    Action atk = GetComponent<ActionBasicAttack>();
                    atk.BeginAction(choice);
                }
                else
                {
                    Action move = GetComponent<ActionMove>();
                    move.BeginAction(choice);
                }
            }
        }
    }

    // Picks an arbitrary attackable target.
    GameObject pickTarget()
    {
        return transform.parent.GetComponent<TurnManager>().PickArbitraryPC();
    }

    Tile AIChooseMove()
    {
        float bestScore = 0.0f;
        Tile bestChoice = null;
        GameObject target = pickTarget();
        foreach (Tile option in selectableTiles)
        {
            if (EvaluateMove(option, target) > bestScore)
            {
                bestScore = EvaluateMove(option, target);
                bestChoice = option;
            }
        }
        if (bestScore > 0.0f) return bestChoice;
        return null;
    }

    // If the tile can be attacked, returns 100. Otherwise,
    // returns 100 minus its distance from the target.
    float EvaluateMove(Tile tile, GameObject target)
    {
        if (ContainsEnemy(tile))
        {
            return 100.0f;
        }
        return 100.0f - Vector3.Distance(tile.transform.position, target.transform.position);
    }

}
