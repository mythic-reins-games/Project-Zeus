using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharsheetUIManager : MonoBehaviour
{

    [SerializeField]
    GameObject LifeText;

    [SerializeField]
    GameObject StrValueText;

    [SerializeField]
    GameObject SpeValueText;

    [SerializeField]
    GameObject EndValueText;

    [SerializeField]
    GameObject AgiValueText;

    [SerializeField]
    GameObject IntValueText;

    [SerializeField]
    GameObject BonusStrValueText;

    [SerializeField]
    GameObject BonusSpeValueText;

    [SerializeField]
    GameObject BonusEndValueText;

    [SerializeField]
    GameObject BonusAgiValueText;

    [SerializeField]
    GameObject BonusIntValueText;

    [SerializeField]
    GameObject NameText;

    [SerializeField]
    GameObject StatusValueText;

    private CharacterSheet characterBeingDisplayed;

    public void Start()
    {
        characterBeingDisplayed = CharsheetUIInitializer.characterBeingDisplayed;
        LifeText.GetComponent<Text>().text = characterBeingDisplayed.currentHealth.ToString() + "/" + characterBeingDisplayed.maxHealth.ToString();
        StrValueText.GetComponent<Text>().text = characterBeingDisplayed.strength.ToString();
        EndValueText.GetComponent<Text>().text = characterBeingDisplayed.endurance.ToString();
        SpeValueText.GetComponent<Text>().text = characterBeingDisplayed.speed.ToString();
        AgiValueText.GetComponent<Text>().text = characterBeingDisplayed.agility.ToString();
        IntValueText.GetComponent<Text>().text = characterBeingDisplayed.intelligence.ToString();
        NameText.GetComponent<Text>().text = characterBeingDisplayed.name;
        StatusValueText.GetComponent<Text>().text = characterBeingDisplayed.selected ? "ACTIVE" : "INACTIVE";
        if (!CharsheetUIInitializer.PCDisplayed)
        {
            Destroy(transform.Find("CharRenameBox").gameObject);
            Destroy(transform.Find("ConfirmRenameButton").gameObject);
            Destroy(transform.Find("StatusToggleButton").gameObject);
        }
    }

    public void RenameActiveCharacter(string NewName)
    {
        characterBeingDisplayed.name = NewName;
        NameText.GetComponent<Text>().text = NewName;
    }

    public void ToggleStatus()
    {
        if (characterBeingDisplayed.selected)
        {
            characterBeingDisplayed.selected = false;
            StatusValueText.GetComponent<Text>().text = "INACTIVE";
        }
        else
        {
            characterBeingDisplayed.selected = true;
            StatusValueText.GetComponent<Text>().text = "ACTIVE";
        }
    }
}
