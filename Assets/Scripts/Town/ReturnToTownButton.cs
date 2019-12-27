using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToTownButton : MonoBehaviour
{
    [SerializeField] GameObject townMenu;

    public void Leave()
    {
        gameObject.transform.parent.gameObject.SetActive(false);
        townMenu.SetActive(true);
    }
}
