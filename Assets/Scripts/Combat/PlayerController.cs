using UnityEngine;

public class PlayerController : CombatController
{

    private Tile hoverTile = null;
    private TurnManager manager = null;

    protected override void Start()
    {
        manager = Object.FindObjectOfType<TurnManager>();
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

    private void CheckMouseClick()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Tile clickedTile = GetMouseTile();
            if (clickedTile == null || !clickedTile.isSelectable) return;
            ClearMouseHover();
            if (clickedTile.occupant != null)
            {
                Action atk = GetComponent<ActionBasicAttack>();
                atk.BeginAction(clickedTile);
            }
            else
            {
                Action move = GetComponent<ActionMove>();
                move.BeginAction(clickedTile);
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
    }
}