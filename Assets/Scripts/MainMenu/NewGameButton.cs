using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class NewGameButton : MonoBehaviour
{

    public void NewGame()
    {
        PlayerParty.Reset();
        EnemyParty.Reset();
        SceneManager.LoadScene(Constants.SCENE_TOWN_MENU);
    }
}
