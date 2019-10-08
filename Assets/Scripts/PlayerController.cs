using UnityEngine;

public class PlayerController : CombatController
{

    void Update()
    {
        if (!isTurn)
        {
            return;
        }
        if (!isActing)
        {
            // Only need to recalculate selectable tiles after action is concluded.
            FindSelectableTiles();
            CheckMouseClick();
        }
    }

    override protected bool CanAttack(Tile tile)
    {
        if (tile.occupant == null) return false;
        return tile.occupant.GetComponent<EnemyController>() != null;
    }

    private void CheckMouseClick()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            // Ignore a click on empty space
            if (!Physics.Raycast(ray, out hit)) return;
            
            if (hit.collider.tag == "Tile")
            {
                Tile clickedTile = hit.collider.GetComponent<Tile>();

                if (!clickedTile.isSelectable) return;
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
        }
    }
}