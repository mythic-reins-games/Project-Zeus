using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    private List<Tile> selectableTiles = new List<Tile>();

    private Tile currentTile;

    public bool isTurn = false;
    protected bool isActing = false;
    [SerializeField] private int move = 5;
    private int actionPoints = 0;

    protected void Init()
    {
        AssignCurrentTile();
    }

    public void BeginTurn()
    {
        isTurn = true;
        actionPoints = move;
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

    public void BeginAction()
    {
        isActing = true;
    }

    public void EndAction(int spentActionPoints)
    {
        RemoveSelectableTiles();
        isActing = false;
        actionPoints -= spentActionPoints;
        AssignCurrentTile();
        if (actionPoints <= 0)
        {
            isTurn = false;
        }
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
