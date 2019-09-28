using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMovement : MonoBehaviour
{
    List<Tile> selectableTiles = new List<Tile>();
    GameObject[] tiles;

    Stack<Tile> path = new Stack<Tile>();
    Tile currentTile;

    public bool isMoving = false;
    public int move = 5;
    public float moveSpeed = 2;

    Vector3 velocity = new Vector3();
    Vector3 direction = new Vector3();

    float halfHeight = 0;

    protected void Init()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");

        halfHeight = GetComponent<CharacterController>().bounds.extents.y;
    }

    public void FindSelectableTiles()
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

    public void FindAdjacentTiles()
    {
        foreach (GameObject tile in tiles)
        {
            Tile tileScript = tile.GetComponent<Tile>();
            tileScript.FindNeighbors();
        }
    }

    public void AssignCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
        currentTile.isCurrent = true;
    }

    // Will need adjustment once there are objects that units can stand on, e.g., crates
    public Tile GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        // Return null if there is nothing below the target
        if (!Physics.Raycast(target.transform.position, Vector3.down, out hit, 1)) return null;
        
        return hit.collider.GetComponent<Tile>();
    }

    public void MoveToTile(Tile tile)
    {
        path.Clear();
        tile.isTarget = true;
        isMoving = true;

        Tile next = tile;
        while (next != null)
        {
            path.Push(next);
            next = next.parent;
        }
    }
}
