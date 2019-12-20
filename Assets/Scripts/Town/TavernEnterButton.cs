using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernEnterButton : MonoBehaviour
{
    [SerializeField]
    GameObject tavernMenu;

    public void EnterTavern()
    {
        tavernMenu.SetActive(true);
        GameObject.Find("MainTownMenu").SetActive(false);
    }
}
