using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMove : Action
{

    [SerializeField] private float moveSpeed = 2;

    protected Stack<Tile> path = new Stack<Tile>();

    // Extra tiles at the end of the action
    protected int reserveTiles = 0;
    protected int freeMoves = 0;

    override protected void Start()
    {
        base.Start();
    }

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
        else
        {
            currentPhase = Phase.NONE;
            EndAction();
        }
    }

    protected void Move()
    {
        if (path.Count > reserveTiles)
        {
            Tile tile = path.Peek();
            Vector3 target = tile.transform.position;

            target.y += 0.08f;

            CharacterController characterController = GetComponent<CharacterController>();

            // transform.position = new Vector3(transform.position.x, target.y + 0.08f, transform.position.z);
            if (Vector3.Distance(transform.position, target) >= 0.12f)
            {
                Vector3 direction = CalculateDirection(target);
                transform.forward = new Vector3(direction.x, 0f, direction.z);
                direction = CalculateDirection(target);
                Vector3 velocity = SetHorizontalVelocity(direction);
                characterController.Move(velocity * Time.deltaTime);
            }
            else
            {
                SoundManager.PlaySound(GetComponent<CreatureMechanics>().footstepSound);
                // Center of tile reached
                transform.position = target;
                SpendMovePoints(tile);
                path.Pop();
            }
            // Putting this under a conditional prevents the creature from "moonwalking" an extra step animation after it reaches the center of tile.
            if (path.Count > reserveTiles)
            {
                anim.SetBool("IsWalking", true);
            }
        }
        else
        {
            anim.SetBool("IsWalking", false);
            currentPhase = Phase.ATTACKING;
        }
    }

    private void SpendMovePoints(Tile tile)
    {
        int c = tile.GetMoveCost();
        if (freeMoves >= c)
        {
            freeMoves -= c;
            return;
        }
        if (freeMoves > 0)
        {
            spentActionPoints += tile.GetMoveCost() - freeMoves;
            freeMoves = 0;
            return;
        }
        spentActionPoints += tile.GetMoveCost();
    }

    override public void BeginAction(Tile targetTile)
    {
        currentPhase = Phase.MOVING;
        CalculatePath(targetTile);
        base.BeginAction(targetTile);
    }

    private void CalculatePath(Tile targetTile)
    {
        path.Clear();
        targetTile.isTarget = true;

        Tile next = targetTile;
        while (next != null)
        {
            path.Push(next);
            next = next.parent;
        }
    }

    protected Vector3 CalculateDirection(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        return direction.normalized;
    }

    private Vector3 SetHorizontalVelocity(Vector3 direction)
    {
        return direction * moveSpeed;
    }
}
