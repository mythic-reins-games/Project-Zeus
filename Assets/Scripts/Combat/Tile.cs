﻿using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private static int DEFAULT_TILE_COST_CURRENT = 0;
    private static int DEFAULT_TILE_COST = 1;
    private static int DEFAULT_TILE_COST_ZONE_CONTROL = 2;

    private static int DEFAULT_TILE_COST_MULTIPLIER = 1;

    public bool isWalkable = true;
    public bool isCurrent = false;
    public bool isBlocked = false;
    public bool isTarget = false;
    public bool isSelectable = false;
    [SerializeField] private int tileCostMultiplier = DEFAULT_TILE_COST_MULTIPLIER;
    private bool isZoneOfControl = false;

    public List<Tile> adjacentTileList = new List<Tile>();

    public GameObject occupant = null;

    // Needed for breadth-first search
    public bool wasVisited = false;
    public Tile parent = null;
    public int distance = 0;

    public void SetIsZoneOfControl(bool zoc)
    {
        isZoneOfControl = zoc;
    }

    void Start()
    {
        FindNeighbors();
    }

    void Update()
    {
        if (isCurrent)
        {
            GetComponent<Renderer>().material.color = Color.magenta;
        }
        else if (isTarget)
        {
            GetComponent<Renderer>().material.color = Color.blue;
        }
        else if (isBlocked)
        {
            if (isSelectable)
            {
                GetComponent<Renderer>().material.color = Color.red;
            } else {
                GetComponent<Renderer>().material.color = new Color(0.5f, 0, 0, 1);
            }
        }
        else if (isSelectable)
        {
            if (tileCostMultiplier != DEFAULT_TILE_COST_MULTIPLIER) {
                GetComponent<Renderer>().material.color = new Color(0, 0.6f, 0, 1);
            }
            else if (isZoneOfControl)
            {
                GetComponent<Renderer>().material.color = new Color(0.1f, 0.8f, 0.1f, 1);
            } else {
                GetComponent<Renderer>().material.color = Color.green;
            }
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.white;
        }
    }

    public bool HasPC()
    {
        return occupant.GetComponent<PlayerController>() != null;
    }

    public bool HasNPC()
    {
        return occupant.GetComponent<EnemyController>() != null;
    }

    public bool HasDestructibleBlocker()
    {
        return occupant.GetComponent<TileBlockerController>().IsStaticBlocker();
    }

    public bool IsFasterParent(Tile newParent)
    {
        return GetTotalDistanceWithParent(newParent) < distance;
    }

    public int GetTotalDistanceWithParent(Tile newParent)
    {
        return newParent.distance + GetMoveCostForParent(newParent);
    }

    private int GetMoveCostForParent(Tile parentTile)
    {
        int tileCost = DEFAULT_TILE_COST;

        if (isCurrent)
        {
            tileCost = DEFAULT_TILE_COST_CURRENT; // You don't pay a move point for entering your current tile.
        }
        else if (parentTile != null && parentTile.isZoneOfControl)
        {
            tileCost = DEFAULT_TILE_COST_ZONE_CONTROL;
        }
        return tileCost * tileCostMultiplier;
    }

    // Cost to enter the tile.
    public int GetMoveCost()
    {
        return GetMoveCostForParent(parent);
    }

    public void ClearMovementVariables()
    {
        isCurrent = false;
        isTarget = false;
        isSelectable = false;
        wasVisited = false;
        parent = null;
        distance = 0;
    }

    public void FindNeighbors()
    {
        CheckTile(Vector3.forward);
        CheckTile(Vector3.back);
        CheckTile(Vector3.right);
        CheckTile(Vector3.left);
    }

    public void Reset()
    {
        ClearMovementVariables();
        adjacentTileList.Clear();
    }

    private void CheckTile(Vector3 direction)
    {
        Vector3 halfExtents = new Vector3(0.25f, 2.0f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

        foreach (Collider item in colliders)
        {
            Tile tile = item.GetComponent<Tile>();

            if (tile == null || !tile.isWalkable) continue;

            adjacentTileList.Add(tile);
        }
    }
}
