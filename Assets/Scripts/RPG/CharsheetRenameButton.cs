using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharsheetRenameButton : MonoBehaviour
{

    [SerializeField]
    GameObject InputBox;

    [SerializeField]
    GameObject UIManager;

    // Renames the character.
    public void RenameClick()
    {
        CharsheetUIManager m = UIManager.GetComponent<CharsheetUIManager>();
        m.RenameActiveCharacter(InputBox.GetComponent<Text>().text);
        PartyIndicator toUpdate = (PartyIndicator)FindObjectOfType(typeof(PartyIndicator));
        toUpdate.PartyCompositionChanged();
    }
}
