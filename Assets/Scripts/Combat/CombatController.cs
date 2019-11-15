using System.Collections.Generic;
using UnityEngine;

public class CombatController : TileBlockerController
{
    private HashSet<Tile> visitedTiles = new HashSet<Tile>();
    protected List<Tile> selectableTiles = new List<Tile>();
    [SerializeField] protected GameSignalOneObject gameSignal;

    protected List<Action> specialMoves = new List<Action> { };

    protected CreatureMechanics creatureMechanics = null;

    protected GUIPanel panel;

    protected bool isActing = false;
    [SerializeField] protected int actionPoints = 0;

    const int ATTACK_COST = 4;

    private enum TileSearchType
    {
        // Default means unit can move at normal per-tile costs and attack at normal attack cost.
        DEFAULT,
        // Attack only means unit can move at normal per-tile costs and attack at normal attack cost.
        // But, moves are only valid if they end in an attack.
        ATTACK_ONLY,
        // Charges get 'free' movement points equal to ATTACK_COST before the character starts paying for movement.
        // But, moves are only valid if they end in an attack.
        CHARGE_ATTACK,
        // Ranged attacks wil raycast to the target, 
        RANGED_ATTACK
    };

    private void RegisterMoves()
    {
        if (GetComponent<ActionRegenerate>() != null) specialMoves.Add(GetComponent<ActionRegenerate>());
        if (GetComponent<ActionBullRush>() != null) specialMoves.Add(GetComponent<ActionBullRush>());
        if (GetComponent<ActionSlaughter>() != null) specialMoves.Add(GetComponent<ActionSlaughter>());
        if (GetComponent<ActionRage>() != null) specialMoves.Add(GetComponent<ActionRage>());
    }

    override protected void Start()
    {
        creatureMechanics = GetComponent<CreatureMechanics>();
        panel = Object.FindObjectOfType<GUIPanel>();
        PopupTextController.Initialize();
        RegisterMoves();
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
        
        actionPoints = creatureMechanics.BeginTurnAndGetMaxActionPoints();
        if (DoesGUI())
        {
            panel.SetActionPoints(actionPoints);
            gameSignal?.Raise(creatureMechanics.GetConcentrationPercent());
        }
        isTurn = true;
        FindSelectableBasicTiles();
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

    // Returns true if valid charge tiles were found.
    protected bool FindSelectableChargeTiles()
    {
        FindSelectableTiles(TileSearchType.CHARGE_ATTACK);
        if (selectableTiles.Count == 0)
        {
            FindSelectableBasicTiles();
            return false;
        }
        return true;
    }

    protected bool FindSelectableAttackTiles()
    {
        FindSelectableTiles(TileSearchType.ATTACK_ONLY);
        if (selectableTiles.Count == 0)
        {
            FindSelectableBasicTiles();
            return false;
        }
        return true;
    }

    protected void FindSelectableBasicTiles()
    {
        FindSelectableTiles(TileSearchType.DEFAULT);
    }

    private void FindSelectableTiles(TileSearchType searchType)
    {
        ClearVisitedTiles();
        AssignCurrentTile();
        if (actionPoints == 0)
        {
            return;
        }
        if (actionPoints < ATTACK_COST && searchType == TileSearchType.ATTACK_ONLY)
        {
            return;
        }
        if (actionPoints < ATTACK_COST && searchType == TileSearchType.CHARGE_ATTACK)
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

            if (tile != currentTile && searchType == TileSearchType.DEFAULT) // Only default can select tiles that end in movement.
            {
                selectableTiles.Add(tile);
                tile.isSelectable = true;
            }

            foreach (Tile adjacentTile in tile.adjacentTileList)
            {
                if (adjacentTile.isBlocked) {
                    if (!adjacentTile.wasVisited)
                    {
                        if (searchType == TileSearchType.CHARGE_ATTACK && tile.distance <= actionPoints && ContainsEnemy(adjacentTile))
                        {
                            AttachTile(ATTACK_COST > tile.distance ? ATTACK_COST : tile.distance, adjacentTile, tile);
                            selectableTiles.Add(adjacentTile);
                            adjacentTile.isSelectable = true;
                        }
                        else if (tile.distance + ATTACK_COST <= actionPoints && ContainsEnemy(adjacentTile))
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
        if (DoesGUI())
        {
            panel.ClearActionPoints();
            gameSignal?.Raise(0);
        }
        isTurn = false;
        currentTile.isCurrent = false;
    }

    public void EndAction(int spentActionPoints)
    {
        if (DoesGUI())
        {
            CreatureMechanics creatureMechanics = GetComponent<CreatureMechanics>();
            panel.SpendActionPoints(spentActionPoints);
            gameSignal?.Raise(creatureMechanics.GetConcentrationPercent());
        }
        isActing = false;
        actionPoints -= spentActionPoints;
        if (transform.parent.GetComponent<TurnManager>().CheckCombatOver())
        {
            isTurn = false;
            return;
        }
        FindSelectableBasicTiles();
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
