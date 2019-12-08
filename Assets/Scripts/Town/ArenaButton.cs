using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaButton : MonoBehaviour
{

    public void EnterCombat()
    {
        Application.LoadLevel(Constants.SCENE_ARENA_COMBAT);
    }
}
