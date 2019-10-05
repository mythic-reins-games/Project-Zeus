using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{

    private List<GameObject> combatants = new List<GameObject>();
    private int moveIdx = 0;

    PlayerController GetCurrentPlayerController()
    {
        return combatants[moveIdx].GetComponent<PlayerController>();
    }

    void NextTurn()
    {
        moveIdx = (moveIdx + 1) % combatants.Count;
        GetCurrentPlayerController().BeginTurn();
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
        GetCurrentPlayerController().BeginTurn();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetCurrentPlayerController().isTurn)
        {
            NextTurn();
        }
    }
}
