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
        //RectTransform rt = instance.GetComponent<RectTransform>();
        //rt.anchorMin = Vector2.zero;
        //rt.anchorMax = Vector2.one;
        //rt.sizeDelta = Vector2.zero;
        //rt.anchoredPosition = Vector2.zero;
    }
}
