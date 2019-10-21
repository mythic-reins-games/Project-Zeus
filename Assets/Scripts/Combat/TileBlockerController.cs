﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBlockerController : MonoBehaviour
{

    public bool isTurn = false;
    protected Tile currentTile;

    virtual protected void Start()
    {
        AssignCurrentTile();
    }

    protected void AssignCurrentTile()
    {
        UnassignCurrentTile();
        currentTile = GetTargetTile(gameObject);
        currentTile.isBlocked = true;
        currentTile.occupant = gameObject;
        if (isTurn)
        {
            currentTile.isCurrent = true;
        }
    }

    public void UnassignCurrentTile()
    {
        if (currentTile != null)
        {
            currentTile.occupant = null;
            currentTile.isBlocked = false;
            currentTile.isCurrent = false;
        }
    }

    // Will need adjustment once there are objects that units can stand on, e.g., crates
    private Tile GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        // Return null if there is nothing below the target
        if (!Physics.Raycast(target.transform.position, Vector3.down, out hit, Mathf.Infinity, Physics.AllLayers))
        {
            return null;
        }
        return hit.collider.GetComponent<Tile>();
    }

}