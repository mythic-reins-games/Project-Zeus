using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EnterCombat : MonoBehaviour
{
    public void Enter()
    {
        if (PlayerParty.CountActivePartyMembers() > 0 && PlayerParty.CountActivePartyMembers() < 4)
            SceneManager.LoadScene(Constants.SCENE_ARENA_COMBAT);
        else
            NotificationPopupSystem.PopupText("This combat requires you to have 1-3 active party members.");
    }
}
