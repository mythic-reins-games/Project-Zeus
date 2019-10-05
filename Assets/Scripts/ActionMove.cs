using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMove : Action
{

    [SerializeField] private float moveSpeed = 2;

    private Stack<Tile> path = new Stack<Tile>();

    // Update is called once per frame
    void Update()
    {
        if (inProgress)
        {
            Move();
        }
    }

    private void Move()
    {
        if (path.Count > 0)
        {
            Tile tile = path.Peek();
            Vector3 target = tile.transform.position;
            
            target.y = 0.08f;

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
                spentActionPoints += 1;
                path.Pop();
            }
        }
        else
        {
            EndAction();
        }
    }

    new public void BeginAction(Tile targetTile)
    {
        CalculatePath(targetTile);
        Move();
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

    private Vector3 CalculateDirection(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        return direction.normalized;
    }

    private Vector3 SetHorizontalVelocity(Vector3 direction)
    {
        return direction * moveSpeed;
    }
}
