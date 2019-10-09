using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TurnManager : MonoBehaviour
{

    private List<GameObject> combatants = new List<GameObject>();
    private int moveIdx = -1;
    private bool EnemyTurn = false;

    CombatController GetCurrentCombatController()
    {
        if (moveIdx == -1) return null;
        if (combatants[moveIdx].GetComponent<PlayerController>() != null)
        {
            EnemyTurn = false;
            return combatants[moveIdx].GetComponent<PlayerController>();
        }
        if (combatants[moveIdx].GetComponent<EnemyController>() != null)
        {
            EnemyTurn = true;
            return combatants[moveIdx].GetComponent<EnemyController>();
        }
        return null;
    }

    // Picks an arbitrary/random Player controlled character
    public GameObject PickArbitraryPC()
    {
        foreach (GameObject pick in combatants)
        {
            if (pick.GetComponent<PlayerController>() != null)
            {
                return pick;
            }
        }
        return null;
    }

    void ClearZonesOfControl()
    {
        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            tile.isZoneOfControl = false;
        }
    }

    void SetZonesOfControl()
    {
        foreach (GameObject combatant in combatants)
        {
            CombatController opponent = null;
            if (EnemyTurn) opponent = combatant.GetComponent<PlayerController>();
            if (!EnemyTurn) opponent = combatant.GetComponent<EnemyController>();
            if (opponent == null) continue;
            opponent.AssignZonesOfControl();
        }
    }

    void BeginTurn()
    {
        CombatController controller = GetCurrentCombatController();
        ClearZonesOfControl();
        SetZonesOfControl();
        controller.BeginTurn();
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child != transform)
            {
                combatants.Add(child.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GetCurrentCombatController() == null)
        {
            moveIdx = (moveIdx + 1) % combatants.Count;
            if (GetCurrentCombatController() != null)
            {
                BeginTurn();
            }
            return;
        }
        if (!GetCurrentCombatController().isTurn)
        {
            moveIdx = (moveIdx + 1) % combatants.Count;
            BeginTurn();
        }
    }
}
