using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ArenaButton : MonoBehaviour
{
    public void EnterCombat()
    {
        EnemyParty.SetArenaFoes();
        SceneManager.LoadScene(Constants.SCENE_PRE_COMBAT);
    }
}
