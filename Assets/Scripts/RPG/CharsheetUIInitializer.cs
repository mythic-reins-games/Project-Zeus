using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CharsheetUIInitializer
{
    public static CharacterSheet characterBeingDisplayed;
    public static bool PCDisplayed;

    public static void DisplayCharacterSheet(CharacterSheet characterToDisplay, bool isPC)
    {
        GameObject charsheetUIPrefab = (GameObject)Resources.Load("Prefabs/UI/CharsheetUIModal", typeof(GameObject));
        characterBeingDisplayed = characterToDisplay;
        GameObject canvas = GameObject.Find("MainUICanvas");
        GameObject instance = GameObject.Instantiate(charsheetUIPrefab, canvas.transform) as GameObject;
        RectTransform rt = instance.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;
        rt.anchoredPosition = Vector2.zero;
        PCDisplayed = isPC;
    }

}
