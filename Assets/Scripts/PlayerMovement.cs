using UnityEngine;

public class PlayerMovement : CombatMovement
{
    void Start()
    {
        Init();
    }

    void Update()
    {
        if (!isMoving)
        {
            FindSelectableTiles();
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