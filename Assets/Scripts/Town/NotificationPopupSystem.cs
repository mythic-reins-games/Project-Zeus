using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NotificationPopupSystem
{

    public static string currentText = "";

    public static void PopupText(string text)
    {
        currentText = text;
        GameObject popupTextPrefab = (GameObject)Resources.Load("Prefabs/NotificationPopup", typeof(GameObject));
        GameObject canvas = GameObject.Find("MainUICanvas");
        GameObject instance = GameObject.Instantiate(popupTextPrefab, canvas.transform) as GameObject;
    }
}
