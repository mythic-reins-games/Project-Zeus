﻿using UnityEditor;
using UnityEngine;

public class PopupTextController : MonoBehaviour
{
    private static PopupText popupTextPrefab;
    private static string canvasName = "Canvas";
    private static GameObject canvas;

    public static void Initialize()
    {
        canvas = GameObject.Find(canvasName);
        popupTextPrefab = canvas.GetComponent<CanvasPrefabs>().popupTextPrefab;
    }

    public static void CreatePopupText(string text, Transform transform)
    {
        PopupText instance = Instantiate(popupTextPrefab);
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

        instance.transform.SetParent(canvas.transform, false);
        instance.transform.position = screenPosition;
        instance.SetText(text);
    }
}
