using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernEnterButton : MonoBehaviour
{
    [SerializeField]
    GameObject tavernMenu;

    [SerializeField]
    GameObject closedTavernMenu;

    public void EnterTavern()
    {
        if (PlayerParty.partyMembers.Count < Constants.MAX_PARTY_SIZE)
            tavernMenu.SetActive(true);
        else
            closedTavernMenu.SetActive(true);
        GameObject.Find("MainTownMenu").SetActive(false);
    }
}
