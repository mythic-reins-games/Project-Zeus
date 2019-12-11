using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernLeaveButton : MonoBehaviour
{
    [SerializeField] GameObject townMenu;

    public void LeaveTavern()
    {
        GameObject.Find("TavernMenu").SetActive(false);
        townMenu.SetActive(true);
    }
}
