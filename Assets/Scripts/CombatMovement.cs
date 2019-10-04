using System.Collections.Generic;
using UnityEngine;

public class CombatMovement : MonoBehaviour
{
    protected List<Tile> selectableTiles = new List<Tile>();
    private GameObject[] tiles;

    private Stack<Tile> path = new Stack<Tile>();
    private Tile currentTile;

    public bool isTurn = false;
    protected bool isMoving = false;
    [SerializeField] private int move = 5;
     private int actionPoints = 0;
    [SerializeField] private float moveSpeed = 2;

    private float unitHalfHeight = 0;

    protected void Init()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");
        unitHalfHeight = GetComponent<CharacterController>().bounds.extents.y;
        AssignCurrentTile();
    }

    public void BeginTurn()
    {
        isTurn = true;
        actionPoints = move;
    }

    protected void EndAction()
    {
        isMoving = false;
        if (actionPoints == 0)
        {
            isTurn = false;
        }
    }

    protected void FindSelectableTiles()
    {
        if (actionPoints == 0)
        {
            return;
        }
        AssignCurrentTile();

        Queue<Tile> queue = new Queue<Tile>();
        currentTile.wasVisited = true;
        queue.Enqueue(currentTile);

        while (queue.Count > 0)
        {
            Tile tile = queue.Dequeue();

            selectableTiles.Add(tile);
            tile.isSelectable = true;
            if (tile.distance >= actionPoints) continue;

            foreach (Tile adjacentTile in tile.adjacentTileList)
            {
                if (adjacentTile.wasVisited) continue;
                if (adjacentTile.isBlocked) continue;
                adjacentTile.distance = 1 + tile.distance;
                adjacentTile.wasVisited = true;
                if (tile == currentTile)
                {
                    adjacentTile.parent = null;
                }
                else
                {
                    adjacentTile.parent = tile;
                }
                queue.Enqueue(adjacentTile);
            }
        }
    }

    private void AssignCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
        currentTile.isBlocked = true;
        currentTile.isCurrent = true;
    }

    // Will need adjustment once there are objects that units can stand on, e.g., crates
    private Tile GetTargetTile(GameObject target)
    {

        RaycastHit hit;
        // Return null if there is nothing below the target
        if (!Physics.Raycast(target.transform.position, Vector3.down, out hit, Mathf.Infinity, Physics.AllLayers))
        {
            return null;
        }

        return hit.collider.GetComponent<Tile>();
    }

    protected void CalculatePath(Tile targetTile)
    {
        path.Clear();
        targetTile.isTarget = true;
        isMoving = true;

        Tile next = targetTile;
        while (next != null)
        {
            path.Push(next);
            next = next.parent;
        }
    }

    protected void Move()
    {
        if (path.Count > 0)
        {
            Tile tile = path.Peek();
            Vector3 target = tile.transform.position;

            // Calculating the unit's position on the target tile, assuming the top of the tile is at y = 0
            target.y = unitHalfHeight;
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
                if (path.Count == 1)
                {
                    currentTile.isBlocked = false;
                    currentTile.isCurrent = false;
                    Debug.Log(currentTile);
                    currentTile = path.Peek();
                    currentTile.isCurrent = true;
                    currentTile.isBlocked = true;
                }
                actionPoints -= 1;
                path.Pop();
            }
        }
        else
        {
            RemoveSelectableTiles();
            EndAction();
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

    private void RemoveSelectableTiles()
    {

        foreach (Tile tile in selectableTiles)
        {
            tile.ClearMovementVariables();
        }

        selectableTiles.Clear();
    }
}
