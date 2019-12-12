using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharsheetContinueButton : MonoBehaviour
{
    // This destroys the entire character sheet modal so the user returns to whatever previous screen they were in.
    public void ContinueClick()
    {
        Destroy(transform.parent.gameObject);
    }
}
