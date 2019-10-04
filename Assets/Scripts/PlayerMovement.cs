using UnityEngine;

public class PlayerMovement : CombatMovement
{
    void Start()
    {
        Init();
    }

    void FixedUpdate()
    {
        if (!isTurn)
        {
            return;
        }
        if (!isMoving)
        {
            // Only need to recalculate selectable tiles after action is concluded.
            if (selectableTiles.Count == 0)
            {
                FindSelectableTiles();
            }
            CheckMouseClick();
        }
        else
        {
            Move();
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

                CalculatePath(clickedTile);
            }
        }
    }
}