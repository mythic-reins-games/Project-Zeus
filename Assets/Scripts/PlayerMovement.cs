using System.Collections;
using System.Collections.Generic;
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
            // TODO Move();
        }
    }

    void CheckMouseClick()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            // Ignore a click on empty space
            if (!Physics.Raycast(ray, out hit)) return;
            
            if (hit.collider.tag == "Tile")
            {
                Tile tile = hit.collider.GetComponent<Tile>();

                if (!tile.isSelectable) return;

                MoveToTile(tile);
            }
        }
    }
}