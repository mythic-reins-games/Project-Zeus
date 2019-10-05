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
                
                ActionMove mover = GetComponent<ActionMove>();
                mover.BeginAction(clickedTile);
            }
        }
    }
}