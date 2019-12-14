using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopEnterButton : MonoBehaviour
{
    [SerializeField]
    GameObject shopMenu;

    public void EnterShop()
    {
        shopMenu.SetActive(true);
        GameObject.Find("MainTownMenu").SetActive(false);
    }
}
