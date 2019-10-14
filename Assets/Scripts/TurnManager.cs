using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TurnManager : MonoBehaviour
{

    [SerializeField] private GameObject combatCamera;
    private List<GameObject> combatants = new List<GameObject>();
    private int moveIdx = -1;
    private bool enemyTurn = false;
    private bool frozen = false;

    CombatController GetCurrentCombatController()
    {
        if (moveIdx == -1) return null;
        if (combatants[moveIdx].GetComponent<PlayerController>() != null)
        {
            enemyTurn = false;
            return combatants[moveIdx].GetComponent<PlayerController>();
        }
        if (combatants[moveIdx].GetComponent<EnemyController>() != null)
        {
            enemyTurn = true;
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
            if (enemyTurn) opponent = combatant.GetComponent<PlayerController>();
            if (!enemyTurn) opponent = combatant.GetComponent<EnemyController>();
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

    private IEnumerator BeginTurnAfterDelay(float fDuration)
    {
        frozen = true;
        float elapsed = 0f;
        while (elapsed < fDuration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        frozen = false;
        BeginTurn();
        yield break;
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
        if (frozen) return;
        if (GetCurrentCombatController() == null)
        {
            moveIdx = (moveIdx + 1) % combatants.Count;
            if (GetCurrentCombatController() != null)
            {
                combatCamera.GetComponent<CombatCamera>().ZoomNear(GetCurrentCombatController());
                StartCoroutine(BeginTurnAfterDelay(0.1f));
            }
            return;
        }
        if (!GetCurrentCombatController().isTurn)
        {
            moveIdx = (moveIdx + 1) % combatants.Count;
            combatCamera.GetComponent<CombatCamera>().ZoomNear(GetCurrentCombatController());
            StartCoroutine(BeginTurnAfterDelay(0.1f));
        }
    }
}
