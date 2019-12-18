using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlayerControllers are intended to be a fairly thin interface between CombatController and
// the UI for Player-Controlled characters.
[RequireComponent(typeof(CreatureMechanics))]
public class PlayerController : ActionValidator
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

    override public bool IsPC()
    {
        return true;
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

    override protected List<CombatController> AllEnemies()
    {
        return manager.AllLivingEnemies();
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

    public string[] AbilityNames()
    {
        string[] names = new string[specialMoves.Count];
        for (int i = 0; i < specialMoves.Count; i++)
        {
            names[i] = specialMoves[i].DisplayName();
        }
        return names;
    }

    public void EndTurnButtonClick()
    {
        if (isTurn && !isActing)
        {
            EndTurn();
        }
    }

    public void ActionClicked(Action action)
    {
        if (selectedAction.GetType() == action.GetType()) // If we've already selected the action, unselect it.
        {
            FindSelectableBasicTiles();
            selectedAction = GetComponent<ActionBasicAttack>();
            return;
        }
        if (!IsValid(action, true)) return;
        FindAllValidTargets(action, true);
    }

    public void AbilityButtonClick(int buttonId)
    {
        if (specialMoves.Count > buttonId)
        {
            ActionClicked(specialMoves[buttonId]);
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
