using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    private HashSet<Tile> visitedTiles = new HashSet<Tile>();
    protected List<Tile> selectableTiles = new List<Tile>();

    private Tile currentTile;

    public bool isTurn = false;
    protected bool isActing = false;
    [SerializeField] private int move = 5;
    [SerializeField] private int actionPoints = 0;

    const int ATTACK_COST = 4;

    protected void Start()
    {
        AssignCurrentTile();
    }
    
    public void AssignZonesOfControl()
    {
        foreach (Tile adjacentTile in currentTile.adjacentTileList)
        {
            adjacentTile.isZoneOfControl = true;
        }
    }

    public void BeginTurn()
    {
        AssignCurrentTile();
        currentTile.isCurrent = true;
        isTurn = true;
        actionPoints = move;
        FindSelectableTiles();
    }

    // Defaults to false, but can be overridden by subclasses.
    // Note that 'enemy' is from the perspective of the actor;
    // for player-controlled, enemies are AI and vice versa.
    virtual protected bool ContainsEnemy(Tile tile)
    {
        return false;
    }

    private void AttachTile(int moveCostOverride, Tile adjacentTile, Tile parent)
    {
        adjacentTile.parent = parent;
        if (moveCostOverride != -1)
        {
            adjacentTile.distance = moveCostOverride + parent.distance;
        }
        else
        {
            adjacentTile.distance = adjacentTile.GetMoveCost() + parent.distance;
        }
    }

    protected void FindSelectableTiles()
    {
        if (actionPoints == 0)
        {
            return;
        }
        AssignCurrentTile();

        // TODO: Replace with PriorityQueue for performance optimization
        List<Tile> queue = new List<Tile>();
        queue.Add(currentTile);
        visitedTiles.Add(currentTile);

        while (queue.Count > 0)
        {
            queue.Sort((item1, item2) => item1.distance.CompareTo(item2.distance));
            Tile tile = queue[0];
            queue.RemoveAt(0);

            if (tile != currentTile)
            {
                selectableTiles.Add(tile);
                tile.isSelectable = true;
            }

            foreach (Tile adjacentTile in tile.adjacentTileList)
            {
                if (adjacentTile.isBlocked) {
                    if (!adjacentTile.wasVisited && ContainsEnemy(adjacentTile) && (tile.distance + ATTACK_COST <= actionPoints))
                    {
                        AttachTile(ATTACK_COST, adjacentTile, tile);
                        visitedTiles.Add(adjacentTile);
                        adjacentTile.wasVisited = true;
                        selectableTiles.Add(adjacentTile);
                        adjacentTile.isSelectable = true;
                    }
                    continue;
                }
                if (!adjacentTile.wasVisited || adjacentTile.IsFasterParent(tile))
                {
                    if (adjacentTile.GetTotalDistanceWithParent(tile) <= actionPoints)
                    {
                        adjacentTile.wasVisited = true;
                        visitedTiles.Add(adjacentTile);
                        AttachTile(-1, adjacentTile, tile);
                        queue.Add(adjacentTile);
                    }
                }
            }
        }
    }

    private void AssignCurrentTile()
    {
        if (currentTile != null)
        {
            currentTile.occupant = null;
            currentTile.isBlocked = false;
            currentTile.isCurrent = false;
        }
        currentTile = GetTargetTile(gameObject);
        currentTile.isBlocked = true;
        currentTile.occupant = gameObject;
        if (isTurn)
        {
            currentTile.isCurrent = true;
        }
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

    protected void EndTurn()
    {
        isTurn = false;
        currentTile.isCurrent = false;
    }

    public void EndAction(int spentActionPoints)
    {
        ClearVisitedTiles();
        actionPoints -= spentActionPoints;
        AssignCurrentTile();
        FindSelectableTiles();
        isActing = false;
        if (selectableTiles.Count <= 0)
        {
            EndTurn();
        }
    }

    private void ClearVisitedTiles()
    {
        selectableTiles.Clear();
        foreach (Tile tile in visitedTiles)
        {
            tile.ClearMovementVariables();
        }

        visitedTiles.Clear();
    }
}
