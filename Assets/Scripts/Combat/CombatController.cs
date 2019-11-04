using System.Collections.Generic;
using UnityEngine;

public class CombatController : TileBlockerController
{
    private HashSet<Tile> visitedTiles = new HashSet<Tile>();
    protected List<Tile> selectableTiles = new List<Tile>();
    [SerializeField] protected GameSignal gameSignal;

    protected GUIPanel panel;

    protected bool isActing = false;
    [SerializeField] private int actionPoints = 0;

    const int ATTACK_COST = 4;

    override protected void Start()
    {
        panel = Object.FindObjectOfType<GUIPanel>();
        PopupTextController.Initialize();
        base.Start();
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
        CreatureStats creatureStats = GetComponent<CreatureStats>();
        actionPoints = creatureStats.GetMaxActionPoints();
        if (DoesGUI())
        {
            panel.SetActionPoints(actionPoints);
            gameSignal?.Raise(creatureStats.GetConcentrationPoints());
        }
        isTurn = true;
        AssignCurrentTile();
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
        currentTile.wasVisited = true;

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
                    if (!adjacentTile.wasVisited)
                    {
                        if (tile.distance + ATTACK_COST <= actionPoints && ContainsEnemy(adjacentTile))
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
                        // You cannot step up a stair of more than .25 meters, though you can jump down any height.
                        if (adjacentTile.transform.position.y > tile.transform.position.y + .25f) continue;
                        adjacentTile.wasVisited = true;
                        visitedTiles.Add(adjacentTile);
                        AttachTile(-1, adjacentTile, tile);
                        queue.Add(adjacentTile);
                    }
                }
            }
        }
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
