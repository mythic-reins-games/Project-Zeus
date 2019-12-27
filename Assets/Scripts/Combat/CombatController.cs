using System.Collections.Generic;
using System.Collections;
using UnityEngine;

// CombatControllers are in charge of determining which moves are valid moves for a unit to take,
// and initializing the execution of those moves.
public class CombatController : TileBlockerController
{
    private HashSet<Tile> visitedTiles = new HashSet<Tile>();
    protected List<Tile> selectableTiles = new List<Tile>();

    protected TurnManager manager = null;

    protected CreatureMechanics creatureMechanics = null;

    protected GUIPanel panel;
    protected Action selectedAction = null;
    public bool isActing = false;
    protected int actionPoints = 0;

    [SerializeField] private TileSearchTypes DEFAULT_ATTACK_TYPE = TileSearchTypes.DEFAULT;

    [SerializeField] protected List<Action> specialMoves = new List<Action> { };

    public enum TileSearchTypes
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
        DEFAULT_RANGED,
        // Reach attacks can attack up to two tiles away (if unblocked), and this move MUST end in an attack.
        REACH_ATTACK_ONLY,
        // Basic reach attack or movement.
        DEFAULT_REACH,
        // Works like ranged attack, but only targets allies.
        ALLY_BUFF
    };

    public List<Action> SpecialMoves
    {
        get => specialMoves;
        set => specialMoves = value;
    }

    public TileSearchTypes TileSearchType
    {
        get => DEFAULT_ATTACK_TYPE;
        set => DEFAULT_ATTACK_TYPE = value;
    }

    public bool Dead()
    {
        // Edge case, controller still being initialized.
        if (creatureMechanics == null) return false;
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

    override protected void Start()
    {
        manager = Object.FindObjectOfType<TurnManager>();
        panel = Object.FindObjectOfType<GUIPanel>();
        PopupTextController.Initialize();
        creatureMechanics = GetComponent<CreatureMechanics>();
        selectedAction = GetComponent<ActionBasicAttack>();
        //gameSignal = (GameSignalOneObject)Resources.Load("Game Signals/SetConcentration", typeof(GameSignalOneObject));
        base.Start();
    }
    
    public void AssignZonesOfControl()
    {
        if (!creatureMechanics.ExertsZoc()) return;
        foreach (Tile adjacentTile in AdjacentTiles())
        {
            adjacentTile.SetIsZoneOfControl(true);
            if (!adjacentTile.isBlocked && DEFAULT_ATTACK_TYPE == TileSearchTypes.DEFAULT_REACH)
            {
                foreach (Tile adjacentAdjacentTile in adjacentTile.adjacentTileList)
                {
                    adjacentAdjacentTile.SetIsZoneOfControl(true);
                }
            }
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
            creatureMechanics.UpdateUI();
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

    // Checks to see if the input tile contains an enemy of this.
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

    virtual protected List<CombatController> AllAllies()
    {
        return null;
    }

    override public void HandleDeath()
    {
        manager.CheckCombatOver();
        manager.ResetZonesOfControl();
        base.HandleDeath();
    }

    private void AttachTile(int moveCostOverride, Tile adjacentTile, Tile parent)
    {
        adjacentTile.parent = parent;
        if (moveCostOverride != -1) // Move cost for attacks or moves.
        {
            adjacentTile.distance = moveCostOverride + parent.distance;
        }
        else
        {
            if (creatureMechanics.IgnoresTerrainCosts())
                adjacentTile.distance = 1 + parent.distance;
            else
                adjacentTile.distance = adjacentTile.GetMoveCost() + parent.distance;
        }
    }

    // Returns true if valid charge tiles were found.
    protected bool FindSelectableChargeTiles(int attackCost)
    {
        FindSelectableTiles(TileSearchTypes.CHARGE_ATTACK, attackCost);
        if (selectableTiles.Count == 0)
        {
            FindSelectableBasicTiles();
            return false;
        }
        return true;
    }

    protected bool FindSelectableAttackTiles(int attackCost)
    {
        FindSelectableTiles(TileSearchTypes.ATTACK_ONLY, attackCost);
        if (selectableTiles.Count == 0)
        {
            FindSelectableBasicTiles();
            return false;
        }
        return true;
    }

    protected bool FindSelectableRangedAttackTiles(int attackCost)
    {
        FindSelectableTiles(TileSearchTypes.RANGED_ATTACK_ONLY, attackCost);
        if (selectableTiles.Count == 0)
        {
            FindSelectableBasicTiles();
            return false;
        }
        return true;
    }

    protected bool FindSelectableAllyBuffTiles(int attackCost)
    {
        FindSelectableTiles(TileSearchTypes.ALLY_BUFF, attackCost);
        if (selectableTiles.Count == 0)
        {
            FindSelectableBasicTiles();
            return false;
        }
        return true;
    }

    protected bool FindSelectableReachAttackTiles(int attackCost)
    {
        FindSelectableTiles(TileSearchTypes.REACH_ATTACK_ONLY, attackCost);
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

    private void AssignRangedAttackTargets(Tile tile, int attackCost, bool targetsFoe)
    {
        if (tile.distance + attackCost > actionPoints) return;
        List<CombatController> allTargets = targetsFoe ? AllEnemies() : AllAllies();
        if (allTargets == null) return;
        foreach (CombatController enemy in allTargets)
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

    private void AssignReachAttackTargets(Tile tile, int attackCost)
    {
        if (tile.distance + attackCost > actionPoints) return;
        foreach (Tile adjacentTile in tile.adjacentTileList)
        {
            if (!adjacentTile.wasVisited && ContainsEnemy(adjacentTile))
            {
                AttachTile(attackCost, adjacentTile, tile);
                selectableTiles.Add(adjacentTile);
                adjacentTile.isSelectable = true;
                visitedTiles.Add(adjacentTile);
                adjacentTile.wasVisited = true;
                continue;
            }
            if (adjacentTile.isBlocked) continue;
            foreach (Tile adjacentAdjacentTile in adjacentTile.adjacentTileList)
            {
                if (!adjacentAdjacentTile.wasVisited && ContainsEnemy(adjacentAdjacentTile))
                {
                    AttachTile(attackCost, adjacentAdjacentTile, tile);
                    selectableTiles.Add(adjacentAdjacentTile);
                    adjacentAdjacentTile.isSelectable = true;
                    visitedTiles.Add(adjacentAdjacentTile);
                    adjacentAdjacentTile.wasVisited = true;
                }
            }
        }
    }

    // Returns true for basic melee types (not reach or ranged).
    private bool IsMeleeType(TileSearchTypes searchType)
    {
        return searchType == TileSearchTypes.DEFAULT ||
            searchType == TileSearchTypes.ATTACK_ONLY ||
            searchType == TileSearchTypes.CHARGE_ATTACK;
    }

    private void FindSelectableTiles(TileSearchTypes searchType, int attackCost = Constants.ATTACK_AP_COST)
    {
        ClearVisitedTiles();
        AssignCurrentTile();
        if (actionPoints == 0)
        {
            return;
        }
        if (actionPoints < attackCost && searchType == TileSearchTypes.ATTACK_ONLY)
        {
            return;
        }
        if (actionPoints < attackCost && searchType == TileSearchTypes.CHARGE_ATTACK)
        {
            return;
        }
        if (actionPoints < attackCost && searchType == TileSearchTypes.RANGED_ATTACK_ONLY)
        {
            return;
        }
        if (actionPoints < attackCost && searchType == TileSearchTypes.REACH_ATTACK_ONLY)
        {
            return;
        }
        if (actionPoints < attackCost && searchType == TileSearchTypes.ALLY_BUFF)
        {
            return;
        }

        // TODO: Replace with PriorityQueue for performance optimization
        List<Tile> queue = new List<Tile>();
        queue.Add(currentTile);
        visitedTiles.Add(currentTile);
        currentTile.wasVisited = true;

        // Can buff self or ally with ally buff.
        if (searchType == TileSearchTypes.ALLY_BUFF)
        {
            selectableTiles.Add(currentTile);
            currentTile.isSelectable = true;
        }

        while (queue.Count > 0)
        {
            queue.Sort((item1, item2) => item1.distance.CompareTo(item2.distance));
            Tile tile = queue[0];
            queue.RemoveAt(0);

            // Only default can select tiles that end in movement.
            if (tile != currentTile && (searchType == TileSearchTypes.DEFAULT || searchType == TileSearchTypes.DEFAULT_RANGED || searchType == TileSearchTypes.DEFAULT_REACH))
            {
                selectableTiles.Add(tile);
                tile.isSelectable = true;
            }

            // Potential ranged attacks
            if (searchType == TileSearchTypes.DEFAULT_RANGED || searchType == TileSearchTypes.RANGED_ATTACK_ONLY)
                AssignRangedAttackTargets(tile, attackCost, true);
            // Potential reach attacks
            if (searchType == TileSearchTypes.DEFAULT_REACH || searchType == TileSearchTypes.REACH_ATTACK_ONLY)
                AssignReachAttackTargets(tile, attackCost);
            // Potential ally buffs
            if (searchType == TileSearchTypes.ALLY_BUFF)
                AssignRangedAttackTargets(tile, attackCost, false);

            foreach (Tile adjacentTile in tile.adjacentTileList)
            {
                if (adjacentTile.isBlocked)
                {
                    // Potential melee attacks.
                    if (!adjacentTile.wasVisited && IsMeleeType(searchType))
                    {
                        if (searchType == TileSearchTypes.CHARGE_ATTACK && tile.distance <= actionPoints && ContainsEnemy(adjacentTile))
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
                // Potential children in search tree.
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
            creatureMechanics.UpdateUI();
        }
        isTurn = false;
        currentTile.isCurrent = false;
    }

    public void EndAction(int spentActionPoints)
    {
        if (DoesGUI())
        {
            panel.SpendActionPoints(spentActionPoints);
            creatureMechanics.UpdateUI();
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
