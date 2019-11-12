using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBasicAttack : ActionMove
{

    override protected void Start()
    {
        // Save one tile at the end of the movement path:
        // this is the tile containing the target enemy.
        reserveTiles = 1;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (!inProgress)
        {
            return;
        }
        if (currentPhase == phase.MOVING)
        {
            Move();
        }
        else if (currentPhase == phase.ATTACKING)
        {
            AttackPhase();
        }
        else
        {
            currentPhase = phase.NONE;
        }
    }

    void ResolveAttack(GameObject target)
    {
        spentActionPoints += 4;
        ObjectStats targetStats = target.GetComponent<CreatureStats>();
        if (targetStats == null) targetStats = target.GetComponent<ObjectStats>();
        AttackEffects(targetStats);
    }

    virtual protected void AttackEffects(ObjectStats targetStats)
    {
        GetComponent<CreatureStats>().PerformBasicAttack(targetStats);
    }

    private IEnumerator WaitForAttackAnimations(float fDuration)
    {
        float elapsed = 0f;
        while (elapsed < fDuration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        EndAction();
        yield break;
    }

    void AttackPhase()
    {
        if (path.Count == 1)
        {
            Tile targetTile = path.Pop();
            Vector3 direction = CalculateDirection(targetTile.transform.position);
            direction.y = 0f;
            transform.forward = direction;
            ResolveAttack(targetTile.occupant);
        }
        else
        {
            currentPhase = phase.NONE;
            StartCoroutine(WaitForAttackAnimations(1.0f));
        }
    }

}
