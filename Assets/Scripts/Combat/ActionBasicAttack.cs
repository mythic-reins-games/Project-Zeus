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

    virtual protected float ATTACK_DURATION { get { return 1.0f; } }

    // Update is called once per frame
    void Update()
    {
        if (!inProgress)
        {
            return;
        }
        if (currentPhase == Phase.MOVING)
        {
            Move();
        }
        else if (currentPhase == Phase.ATTACKING)
        {
            AttackPhase();
        }
        else
        {
            currentPhase = Phase.NONE;
        }
    }

    void ResolveAttack(GameObject target)
    {
        spentActionPoints += 4;
        ObjectMechanics targetMechanics = target.GetComponent<CreatureMechanics>();
        if (targetMechanics == null) targetMechanics = target.GetComponent<ObjectMechanics>();
        AttackEffects(targetMechanics);
    }

    virtual protected void AttackEffects(ObjectMechanics targetStats)
    {
        mechanics.PerformBasicAttack(targetStats);
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
            currentPhase = Phase.NONE;
            StartCoroutine(WaitForAttackAnimations(ATTACK_DURATION));
        }
    }

}
