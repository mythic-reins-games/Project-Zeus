using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    protected List<Tile> selectableTiles = new List<Tile>();

    private Tile currentTile;

    public bool isTurn = false;
    protected bool isActing = false;
    [SerializeField] private int move = 5;
    private int actionPoints = 0;

    const int ATTACK_COST = 4;

    protected void Start()
    {
        AssignCurrentTile();
    }

    public void BeginTurn()
    {
        AssignCurrentTile();
        currentTile.isCurrent = true;
        isTurn = true;
        actionPoints = move;
    }

    // Defaults to false, but can be overridden by subclasses.
    virtual protected bool CanAttack(Tile tile)
    {
        return false;
    }

    private void AttachTile(int moveCost, Tile adjacentTile, Tile parent)
    {
        if (parent != currentTile)
        {
            adjacentTile.parent = parent;
        }
        adjacentTile.distance = moveCost + parent.distance;
        adjacentTile.wasVisited = true;
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
                if (adjacentTile.isBlocked) {
                    if (CanAttack(adjacentTile) && (tile.distance + ATTACK_COST <= actionPoints))
                    {
                        AttachTile(ATTACK_COST, adjacentTile, tile);
                        selectableTiles.Add(adjacentTile);
                        adjacentTile.isSelectable = true;
                    }
                    continue;
                }
                // Basic moves cost one action point per tile.
                AttachTile(1, adjacentTile, tile);
                queue.Enqueue(adjacentTile);
            }
        }
    }

    private void AssignCurrentTile()
    {
        if (currentTile != null)
        {
            currentTile.occupant = null;
            currentTile.isBlocked = false;
        }
        currentTile = GetTargetTile(gameObject);
        currentTile.isBlocked = true;
        currentTile.occupant = gameObject;
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
        RemoveSelectableTiles();
        isActing = false;
        actionPoints -= spentActionPoints;
        AssignCurrentTile();
        if (actionPoints <= 0)
        {
            EndTurn();
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
