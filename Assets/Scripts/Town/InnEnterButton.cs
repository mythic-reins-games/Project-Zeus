using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnEnterButton : MonoBehaviour
{

    [SerializeField]
    GameObject innMenu;

    public void EnterInn()
    {
        innMenu.SetActive(true);
        GameObject.Find("MainTownMenu").SetActive(false);
    }
}
