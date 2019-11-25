using System.Collections.Generic;
using UnityEngine;

// CombatControllers are in charge of determining which moves are valid moves for a unit to take,
// and initializing the execution of those moves.
public class CombatController : TileBlockerController
{
    private HashSet<Tile> visitedTiles = new HashSet<Tile>();
    protected List<Tile> selectableTiles = new List<Tile>();
    [SerializeField] protected GameSignalOneObject gameSignal;

    protected List<Action> specialMoves = new List<Action> { };
    protected TurnManager manager = null;

    protected CreatureMechanics creatureMechanics = null;

    protected GUIPanel panel;
    protected Action selectedAction = null;
    public bool isActing = false;
    protected int actionPoints = 0;

    [SerializeField]
    private TileSearchType DEFAULT_ATTACK_TYPE = TileSearchType.DEFAULT;

    private enum TileSearchType
    {
        // Default means unit can move at normal per-tile costs and melee attack at normal attack cost.
        DEFAULT,
        // Attack only means unit can move at normal per-tile costs and attack at normal attack cost.
        // But, moves are only valid if they end in an attack.
        ATTACK_ONLY,
        // Charges get 'free' movement points equal to Constants.ATTACK_AP_COST before the character starts paying for movement.
        // But, moves are only valid if they end in an attack.
        CHARGE_ATTACK,
        // Ranged attacks wil raycast to the target, and this move MUST end in an attack.
        RANGED_ATTACK_ONLY,
        // Basic ranged attack or movement.
        DEFAULT_RANGED
    };

    public bool Dead()
    {
        return creatureMechanics.dead;
    }

    virtual public bool IsEnemy()
    {
        return false;
    }

    virtual public bool IsPC()
    {
        return false;
    }

    private void RegisterMoves()
    {
        if (GetComponent<ActionBullRush>() != null) specialMoves.Add(GetComponent<ActionBullRush>());
        if (GetComponent<ActionSlaughter>() != null) specialMoves.Add(GetComponent<ActionSlaughter>());
        if (GetComponent<ActionRage>() != null) specialMoves.Add(GetComponent<ActionRage>());
        if (GetComponent<ActionRegenerate>() != null) specialMoves.Add(GetComponent<ActionRegenerate>());
        if (GetComponent<ActionMultiAttack>() != null) specialMoves.Add(GetComponent<ActionMultiAttack>());
        if (GetComponent<ActionEmpower>() != null) specialMoves.Add(GetComponent<ActionEmpower>());
        if (GetComponent<ActionOffhandAttack>() != null) specialMoves.Add(GetComponent<ActionOffhandAttack>());
        if (GetComponent<ActionLifeOrDeath>() != null) specialMoves.Add(GetComponent<ActionLifeOrDeath>());
        if (GetComponent<ActionPetrify>() != null) specialMoves.Add(GetComponent<ActionPetrify>());
        if (GetComponent<ActionTailSweep>() != null) specialMoves.Add(GetComponent<ActionTailSweep>());
        if (GetComponent<ActionSnakeBite>() != null) specialMoves.Add(GetComponent<ActionSnakeBite>());
        if (GetComponent<ActionTerrify>() != null) specialMoves.Add(GetComponent<ActionTerrify>());
    }

    override protected void Start()
    {
        manager = transform.parent.GetComponent<TurnManager>();
        creatureMechanics = GetComponent<CreatureMechanics>();
        panel = Object.FindObjectOfType<GUIPanel>();
        PopupTextController.Initialize();
        RegisterMoves();
        selectedAction = GetComponent<ActionBasicAttack>();
        base.Start();
    }
    
    public void AssignZonesOfControl()
    {
        foreach (Tile adjacentTile in AdjacentTiles())
        {
            adjacentTile.SetIsZoneOfControl(true);
        }
    }

    public List<Tile> AdjacentTiles()
    {
        return currentTile.adjacentTileList;
    }

    private void CooldownSpecialMoves()
    {
        foreach (Action a in specialMoves)
        {
            a.AdvanceCooldown();
        }
    }

    public void BeginTurn()
    {
        CooldownSpecialMoves();
        actionPoints = creatureMechanics.BeginTurnAndGetMaxActionPoints();
        // creatureMechanics.BeginTurn... can kill a unit (example: poison, burning), so we need to check
        // for death after beginning turn.
        if (Dead()) return;
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

    // Defaults to false, but can be overridden by subclasses.
    // Note that 'enemy' is from the perspective of the actor;
    // for player-controlled, enemies are AI and vice versa.
    virtual protected List<CombatController> AllEnemies()
    {
        return null;
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
    protected bool FindSelectableChargeTiles(int attackCost)
    {
        FindSelectableTiles(TileSearchType.CHARGE_ATTACK, attackCost);
        if (selectableTiles.Count == 0)
        {
            FindSelectableBasicTiles();
            return false;
        }
        return true;
    }

    protected bool FindSelectableAttackTiles(int attackCost)
    {
        FindSelectableTiles(TileSearchType.ATTACK_ONLY, attackCost);
        if (selectableTiles.Count == 0)
        {
            FindSelectableBasicTiles();
            return false;
        }
        return true;
    }

    protected bool FindSelectableRangedAttackTiles(int attackCost)
    {
        FindSelectableTiles(TileSearchType.RANGED_ATTACK_ONLY, attackCost);
        if (selectableTiles.Count == 0)
        {
            FindSelectableBasicTiles();
            return false;
        }
        return true;
    }

    protected void FindSelectableBasicTiles()
    {
        FindSelectableTiles(DEFAULT_ATTACK_TYPE);
    }

    private bool RangedAttackValid(Tile originTile, GameObject target)
    {
        Vector3 originPos = originTile.transform.position;
        Vector3 targetPos = target.transform.position;
        originPos.y += 0.5f;
        targetPos.y += 0.5f;
        RaycastHit hit;
        if (Physics.Linecast(originPos, targetPos, out hit))
        {
            if (hit.collider.gameObject == target)
            {
                return true;
            }
        }
        return false;
    }

    private void AssignRangedAttackTargets(Tile tile, int attackCost)
    {
        if (tile.distance + attackCost > actionPoints) return;
        if (AllEnemies() == null) return;
        foreach (CombatController enemy in AllEnemies())
        {
            Tile enemyTile = enemy.currentTile;
            if (!enemyTile.wasVisited && RangedAttackValid(tile, enemy.gameObject))
            {
                AttachTile(attackCost, enemyTile, tile);
                selectableTiles.Add(enemyTile);
                enemyTile.isSelectable = true;
                visitedTiles.Add(enemyTile);
                enemyTile.wasVisited = true;
            }
        }
    }

    private void FindSelectableTiles(TileSearchType searchType, int attackCost = Constants.ATTACK_AP_COST)
    {
        ClearVisitedTiles();
        AssignCurrentTile();
        if (actionPoints == 0)
        {
            return;
        }
        if (actionPoints < attackCost && searchType == TileSearchType.ATTACK_ONLY)
        {
            return;
        }
        if (actionPoints < attackCost && searchType == TileSearchType.CHARGE_ATTACK)
        {
            return;
        }
        if (actionPoints < attackCost && searchType == TileSearchType.RANGED_ATTACK_ONLY)
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

            // Only default can select tiles that end in movement.
            if (tile != currentTile && (searchType == TileSearchType.DEFAULT || searchType == TileSearchType.DEFAULT_RANGED))
            {
                selectableTiles.Add(tile);
                tile.isSelectable = true;
            }

            // Potential ranged attacks
            if (searchType == TileSearchType.DEFAULT_RANGED || searchType == TileSearchType.RANGED_ATTACK_ONLY)
                AssignRangedAttackTargets(tile, attackCost);

            foreach (Tile adjacentTile in tile.adjacentTileList)
            {
                if (adjacentTile.isBlocked)
                {
                    // Potential melee attacks.
                    if (!adjacentTile.wasVisited && searchType != TileSearchType.DEFAULT_RANGED && searchType != TileSearchType.RANGED_ATTACK_ONLY)
                    {
                        if (searchType == TileSearchType.CHARGE_ATTACK && tile.distance <= actionPoints && ContainsEnemy(adjacentTile))
                        {
                            AttachTile(attackCost > tile.distance ? attackCost : tile.distance, adjacentTile, tile);
                            selectableTiles.Add(adjacentTile);
                            adjacentTile.isSelectable = true;
                        }
                        else if (tile.distance + attackCost <= actionPoints && ContainsEnemy(adjacentTile))
                        {
                            AttachTile(attackCost, adjacentTile, tile);
                            selectableTiles.Add(adjacentTile);
                            adjacentTile.isSelectable = true;
                        }
                        visitedTiles.Add(adjacentTile);
                        adjacentTile.wasVisited = true;
                    }
                    continue;
                }
                // Potential movement.
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
        selectedAction = GetComponent<ActionBasicAttack>();
        isActing = false;
        actionPoints -= spentActionPoints;
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
