using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMove : Action
{

    [SerializeField] private float moveSpeed = 2;

    protected Stack<Tile> path = new Stack<Tile>();

    // Extra tiles at the end of the action
    protected int reserve_tiles = 0;

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
        if (currentPhase == phase.MOVING)
        {
            Move();
        }
        else
        {
            currentPhase = 0;
            EndAction();
        }
    }

    protected void Move()
    {
        if (path.Count > reserve_tiles)
        {
            Tile tile = path.Peek();
            Vector3 target = tile.transform.position;
            
            target.y += 0.08f;

            if (Vector3.Distance(transform.position, target) >= 0.1f)
            {
                CharacterController characterController = GetComponent<CharacterController>();
                Vector3 direction = CalculateDirection(target);
                Vector3 velocity = SetHorizontalVelocity(direction);
                transform.forward = direction;
                characterController.Move(velocity * Time.deltaTime);
            }
            else
            {
                // Center of tile reached
                transform.position = target;
                spentActionPoints += tile.GetMoveCost();
                path.Pop();
            }
            // Putting this under a conditional prevents the creature from "moonwalking" an extra step animation after it reaches the center of tile.
            if (path.Count > reserve_tiles)
            {
                anim.SetBool("IsWalking", true);
            }
        }
        else
        {
            anim.SetBool("IsWalking", false);
            currentPhase = phase.ATTACKING;
        }
    }

    override public void BeginAction(Tile targetTile)
    {
        currentPhase = phase.MOVING;
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
