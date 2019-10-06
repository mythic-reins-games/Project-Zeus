using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CombatController
{

    // Update is called once per frame
    void Update()
    {
        // For now, the AI will just skip its turns.
        if (!isTurn)
        {
            return;
        }
        isTurn = false;
    }
}
