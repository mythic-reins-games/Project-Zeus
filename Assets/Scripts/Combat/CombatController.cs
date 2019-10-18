using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    private HashSet<Tile> visitedTiles = new HashSet<Tile>();
    protected List<Tile> selectableTiles = new List<Tile>();

    protected GUIPanel panel;

    private Tile currentTile;

    public bool isTurn = false;
    protected bool isActing = false;
    [SerializeField] private int actionPoints = 0;

    const int ATTACK_COST = 4;

    protected void Start()
    {
        panel = Object.FindObjectOfType<GUIPanel>();
        AssignCurrentTile();
        PopupTextController.Initialize();
    }
    
    public void AssignZonesOfControl()
    {
        foreach (Tile adjacentTile in currentTile.adjacentTileList)
        {
            adjacentTile.SetIsZoneOfControl(true);
        }
    }

    public void BeginTurn()
    {
        actionPoints = GetComponent<CreatureStats>().GetMaxActionPoints();
        if (DoesGUI()) panel.SetActionPoints(actionPoints);
        AssignCurrentTile();
        currentTile.isCurrent = true;
        isTurn = true;
        FindSelectableTiles();
    }

    // Defaults to false, but can be overridden by subclasses.
    // If true, the unit is interactable via the GUI.
    virtual protected bool DoesGUI()
    {
        return false;
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
        AssignCurrentTile();
        if (actionPoints == 0)
        {
            return;
        }

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
                    if (!adjacentTile.wasVisited && ContainsEnemy(adjacentTile))
                    {
                        if (tile.distance + ATTACK_COST <= actionPoints)
                        {
                            AttachTile(ATTACK_COST, adjacentTile, tile);
                            selectableTiles.Add(adjacentTile);
                            adjacentTile.isSelectable = true;
                        }
                        visitedTiles.Add(adjacentTile);
                        adjacentTile.wasVisited = true;
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

    public void UnassignCurrentTile()
    {
        if (currentTile != null) {
            currentTile.occupant = null;
            currentTile.isBlocked = false;
            currentTile.isCurrent = false;
        }
    }

    private void AssignCurrentTile()
    {
        UnassignCurrentTile();
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
        // Since we might have 'visited' a tile in FindSelectableTiles, we need to re-clear.
        ClearVisitedTiles();
        if (DoesGUI()) panel.ClearActionPoints();
        isTurn = false;
        currentTile.isCurrent = false;
    }

    public void EndAction(int spentActionPoints)
    {
        if (DoesGUI()) panel.SpendActionPoints(spentActionPoints);
        isActing = false;
        ClearVisitedTiles();
        actionPoints -= spentActionPoints;
        if (transform.parent.GetComponent<TurnManager>().CheckCombatOver())
        {
            isTurn = false;
            return;
        }
        FindSelectableTiles();
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
