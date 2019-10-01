using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{

    private List<GameObject> combatants = new List<GameObject>();
    private int moveIdx;

    void NextTurn()
    {
        moveIdx = (moveIdx + 1) % combatants.Count;
        combatants[moveIdx].GetComponent<PlayerMovement>().BeginTurn();
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
        combatants[0].GetComponent<PlayerMovement>().BeginTurn();
    }

    // Update is called once per frame
    void Update()
    {
        if (!combatants[moveIdx].GetComponent<PlayerMovement>().isTurn)
        {
            NextTurn();
        }
    }
}
