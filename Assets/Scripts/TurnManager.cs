using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{

    private List<GameObject> combatants = new List<GameObject>();
    private int moveIdx;

    PlayerMovement GetCurrentPlayerMovement()
    {
        return combatants[moveIdx].GetComponent<PlayerMovement>();
    }

    void NextTurn()
    {
        moveIdx = (moveIdx + 1) % combatants.Count;
        GetCurrentPlayerMovement().BeginTurn();
    }

    // Start is called before the first frame update
    void Start()
    {
        moveIdx = 0;
        foreach (Transform child in transform)
        {
            if (child != transform)
            {
                combatants.Add(child.gameObject);
            }
        }
        GetCurrentPlayerMovement().BeginTurn();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetCurrentPlayerMovement().isTurn)
        {
            NextTurn();
        }
    }
}
