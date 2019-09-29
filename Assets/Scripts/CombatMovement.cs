using System.Collections.Generic;
using UnityEngine;

public class CombatMovement : MonoBehaviour
{
    private List<Tile> selectableTiles = new List<Tile>();
    private GameObject[] tiles;

    private Stack<Tile> path = new Stack<Tile>();
    private Tile currentTile;

    protected bool isMoving = false;
    [SerializeField] private int move = 5;
    [SerializeField] private float moveSpeed = 2;

    private float unitHalfHeight = 0;

    protected void Init()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");

        unitHalfHeight = GetComponent<CharacterController>().bounds.extents.y;
    }

    protected void FindSelectableTiles()
    {
        FindAdjacentTiles();
        AssignCurrentTile();

        Queue<Tile> queue = new Queue<Tile>();

        queue.Enqueue(currentTile);
        currentTile.wasVisited = true;

        while (queue.Count > 0)
        {
            Tile tile = queue.Dequeue();

            selectableTiles.Add(tile);
            tile.isSelectable = true;

            if (tile.distance >= move) continue;

            foreach (Tile adjacentTile in tile.adjacentTileList)
            {
                if (adjacentTile.wasVisited) continue;

                adjacentTile.parent = tile;
                adjacentTile.wasVisited = true;
                adjacentTile.distance = 1 + tile.distance;
                queue.Enqueue(adjacentTile);
            }
        }
    }

    private void FindAdjacentTiles()
    {
        foreach (GameObject tileObject in tiles)
        {
            Tile tile = tileObject.GetComponent<Tile>();
            tile.FindNeighbors();
        }
    }

    private void AssignCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
        currentTile.isCurrent = true;
    }

    // Will need adjustment once there are objects that units can stand on, e.g., crates
    private Tile GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        // Return null if there is nothing below the target
        if (!Physics.Raycast(target.transform.position, Vector3.down, out hit, 1)) return null;
        
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

            if (Vector3.Distance(transform.position, target) >= 0.05f)
            {
                Vector3 direction = CalculateDirection(target);
                Vector3 velocity = SetHorizontalVelocity(direction);

                transform.forward = direction;
                transform.position += velocity * Time.deltaTime;
            }
            else
            {
                // Center of tile reached
                transform.position = target;
                path.Pop();
            }
        }
        else
        {
            RemoveSelectableTiles();
            isMoving = false;
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
        if (currentTile != null)
        {
            currentTile.isCurrent = false;
            currentTile = null;
        }

        foreach (Tile tile in selectableTiles)
        {
            tile.Reset();
        }

        selectableTiles.Clear();
    }
}
