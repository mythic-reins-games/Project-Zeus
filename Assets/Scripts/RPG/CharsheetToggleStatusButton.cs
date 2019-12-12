using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharsheetToggleStatusButton : MonoBehaviour
{
    [SerializeField]
    GameObject UIManager;

    public void ToggleStatus()
    {
        CharsheetUIManager m = UIManager.GetComponent<CharsheetUIManager>();
        m.ToggleStatus();
    }
}
