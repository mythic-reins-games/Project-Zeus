using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueFromRewards : MonoBehaviour
{
    public void ReturnToTown() {
        PlayerParty.ResetPowerUpTexts();
        SceneManager.LoadScene(Constants.SCENE_TOWN_MENU);
    }
}
