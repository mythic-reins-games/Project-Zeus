using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernEnterButton : MonoBehaviour
{
    [SerializeField]
    GameObject tavernMenu;

    public void EnterTavern()
    {
        GameObject.Find("MainTownMenu").SetActive(false);
        tavernMenu.SetActive(true);
    }
}
