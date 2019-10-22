using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class TurnManager : MonoBehaviour
{

    System.Random rng;
    [SerializeField] private GameObject combatCamera;
    private List<GameObject> combatants = new List<GameObject>();
    private int moveIdx = -1;
    private bool enemyTurn = false;
    private bool frozen = false;
    private bool gameOver = false;
    private GUIPanel panel = null;

    void Start()
    {
        panel = GameObject.FindObjectOfType<GUIPanel>();
        rng = new System.Random();
        foreach (Transform child in transform)
        {
            if (child != transform)
            {
                combatants.Add(child.gameObject);
            }
        }
    }

    CreatureStats GetCurrentCreatureStats()
    {
        if (moveIdx == -1) return null;
        if (combatants[moveIdx] == null) return null;
        return combatants[moveIdx].GetComponent<CreatureStats>();
    }

    CombatController GetCurrentCombatController()
    {
        if (moveIdx == -1) return null;
        if (combatants[moveIdx] == null) return null;
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

    void EndDefeat()
    {
        MusicManager m = GameObject.Find("MusicManager").GetComponent<MusicManager>();
        gameOver = true;
        GetCurrentCombatController().isTurn = false;
        m.SetDefeat();
    }

    void EndVictory()
    {
        MusicManager m = GameObject.Find("MusicManager").GetComponent<MusicManager>();
        gameOver = true;
        GetCurrentCombatController().isTurn = false;
        m.SetVictory();
    }

    bool EnemyWon()
    {
        foreach (GameObject pick in combatants)
        {
            if (pick == null) continue;
            if (pick.GetComponent<PlayerController>() != null) return false;
        }
        return true;
    }

    bool PlayerWon()
    {
        foreach (GameObject pick in combatants)
        {
            if (pick == null) continue;
            if (pick.GetComponent<EnemyController>() != null) return false;
        }
        return true;
    }

    public bool CheckCombatOver()
    {
        if (PlayerWon())
        {
            EndVictory();
            return true;
        }
        if (EnemyWon())
        {
            EndDefeat();
            return true;
        }
        return false;
    }

    private int CountPCs()
    {
        int count = 0;
        foreach (GameObject pick in combatants)
        {
            if (pick == null) continue;
            if (pick.GetComponent<PlayerController>() != null)
            {
                count++;
            }
        }
        return count;
    }

    // Picks an arbitrary/random Player controlled character
    public GameObject PickArbitraryPC()
    {
        int randomChoice = rng.Next(0, CountPCs());
        foreach (GameObject pick in combatants)
        {
            if (pick == null) continue;
            if (pick.GetComponent<PlayerController>() != null)
            {
                if (randomChoice == 0) return pick;
                randomChoice--;
            }
        }
        return null;
    }

    void ClearZonesOfControl()
    {
        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            tile.SetIsZoneOfControl(false);
        }
    }

    void SetZonesOfControl()
    {
        foreach (GameObject combatant in combatants)
        {
            if (combatant == null) continue;
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
        panel.DisplayStats(GetCurrentCreatureStats());
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

    void AdvanceToNextTurn()
    {
        moveIdx = (moveIdx + 1) % combatants.Count;
        if (GetCurrentCombatController() != null)
        {
            combatCamera.GetComponent<CombatCamera>().ZoomNear(GetCurrentCombatController());
            StartCoroutine(BeginTurnAfterDelay(0.1f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (frozen || gameOver) return;
        if (GetCurrentCombatController() == null || !GetCurrentCombatController().isTurn)
        {
            AdvanceToNextTurn();
        }
    }
}
