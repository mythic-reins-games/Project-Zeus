using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIndicatorClick : MonoBehaviour
{
    public bool isPC;
    public CharacterSheet toDisplay;

    public void OpenCharsheetUI()
    {
        CharsheetUIInitializer.DisplayCharacterSheet(toDisplay, isPC);
    }
}
