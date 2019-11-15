using UnityEngine;

public class PlayerController : CombatController
{
    private static readonly KeyCode[] KEY_CODES = {
         KeyCode.Alpha1,
         KeyCode.Alpha2,
         KeyCode.Alpha3,
         KeyCode.Alpha4,
         KeyCode.Alpha5,
         KeyCode.Alpha6,
         KeyCode.Alpha7,
         KeyCode.Alpha8,
         KeyCode.Alpha9,
     };

    private Tile hoverTile = null;
    private TurnManager manager = null;
    private Action selectedAction = null;

    protected override void Start()
    {
        manager = Object.FindObjectOfType<TurnManager>();
        selectedAction = GetComponent<ActionBasicAttack>();
        base.Start();
    }

    void Update()
    {
        if (!isTurn)
        {
            return;
        }
        if (!isActing)
        {
            CheckMouseClick();
        }
    }

    override public bool IsStaticBlocker()
    {
        return false;
    }

    override protected bool DoesGUI()
    {
        return true;
    }

    override protected bool ContainsEnemy(Tile tile)
    {
        if (tile.occupant == null) return false;
        return tile.HasNPC() || tile.HasDestructibleBlocker();
    }

    private Tile GetMouseTile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray, 100.0f);

        foreach (RaycastHit hit in hits)
        { 
            if (hit.collider.tag == "Tile")
            {
                Tile mouseTile = hit.collider.GetComponent<Tile>();
                return mouseTile;
            }
        }
        return null;
    }

    void ClearMouseHover()
    {
        foreach (GameObject line in GameObject.FindGameObjectsWithTag("LineTag"))
        {
            Destroy(line);
        }
        manager.DisplayCurrentCreatureStats();
    }

    void LineBetweenPositions(Vector3 start, Vector3 end)
    {
        GameObject lineObject = new GameObject("Line");
        lineObject.tag = "LineTag";
        LineRenderer line = lineObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
        line.positionCount = 2;
        line.startWidth = 0.2f;
        line.endWidth = 0.2f;
        Vector3[] points = new Vector3[2];
        points[0] = start;
        points[1] = end;
        line.SetPositions(points);
    }

    private void SetMouseHover()
    {
        hoverTile = GetMouseTile();
        if (hoverTile == null) return;
        if (hoverTile.occupant != null && !hoverTile.HasDestructibleBlocker())
        {
            manager.DisplayCreatureStats(hoverTile.occupant);
        }
        Tile t = hoverTile;
        while (t.parent)
        {
            LineBetweenPositions(t.transform.position, t.parent.transform.position);
            t = t.parent;
        }
    }

    public void EndTurnButtonClick()
    {
        if (isTurn && !isActing)
        {
            EndTurn();
        }
    }

    private void ActivateSelfSpecialMove(Action action)
    {
        action.BeginAction(null);
    }

    private void TargetChargeSpecialMove(Action action)
    {
        if (selectedAction.GetType() == action.GetType()) // If we've already selected the action, unselect it.
        {
            FindSelectableBasicTiles();
            action = GetComponent<ActionBasicAttack>();
            return;
        }
        if (FindSelectableChargeTiles())
        {
            selectedAction = action;
        }
        else
        {
            creatureMechanics.DisplayPopup("Nothing in range");
        }
    }

    private void TargetMeleeSpecialMove(Action action)
    {
        if (selectedAction.GetType() == action.GetType()) // If we've already selected the action, unselect it.
        {
            FindSelectableBasicTiles();
            action = GetComponent<ActionBasicAttack>();
            return;
        }
        if (FindSelectableAttackTiles())
        {
            selectedAction = action;
        }
        else
        {
            creatureMechanics.DisplayPopup("Nothing in range");
        }
    }

    public void ActionClicked(Action action)
    {
        if (actionPoints < action.MIN_AP_COST)
        {
            creatureMechanics.DisplayPopup("Not enough AP");
            return;
        }
        if (creatureMechanics.currentConcentration < action.CONCENTRATION_COST)
        {
            creatureMechanics.DisplayPopup("Not enough concentration");
            return;
        }
        switch (action.TARGET_TYPE)
        {
            case Action.TargetType.SELF_ONLY:
                ActivateSelfSpecialMove(action);
                break;
            case Action.TargetType.CHARGE:
                TargetChargeSpecialMove(action);
                break;
            case Action.TargetType.MELEE:
                TargetMeleeSpecialMove(action);
                break;
        }
    }

    private void CheckMouseClick()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Tile clickedTile = GetMouseTile();
            if (clickedTile == null || !clickedTile.isSelectable) return;
            ClearMouseHover();
            if (clickedTile.occupant != null)
            {
                selectedAction.BeginAction(clickedTile);
                selectedAction = GetComponent<ActionBasicAttack>();
                return;
            }
            else
            {
                Action move = GetComponent<ActionMove>();
                move.BeginAction(clickedTile);
                return;
            }
        }
        else
        {
            if (GetMouseTile() != hoverTile)
            {
                ClearMouseHover();
                SetMouseHover();
            }
        }

        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown(KEY_CODES[i]) && specialMoves.Count > i)
            {
                ActionClicked(specialMoves[i]);
                return;
            }
        }
    }
}