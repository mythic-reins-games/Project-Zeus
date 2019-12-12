using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernLeaveButton : MonoBehaviour
{
    [SerializeField] GameObject townMenu;

    public void LeaveTavern()
    {
        gameObject.transform.parent.gameObject.SetActive(false);
        townMenu.SetActive(true);
    }
}
