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
            Destroy(transform.Find("Equipment").gameObject);
        }
        UpdateBonuses();
    }

    private string ClearIfZero(int toProcess)
    {
        if (toProcess == 0) return "";
        return "+" + toProcess.ToString();
    }

    public void UpdateBonuses()
    {
        BonusStrValueText.GetComponent<Text>().text = ClearIfZero(characterBeingDisplayed.bonusStrength);
        BonusEndValueText.GetComponent<Text>().text = ClearIfZero(characterBeingDisplayed.bonusEndurance);
        BonusSpeValueText.GetComponent<Text>().text = ClearIfZero(characterBeingDisplayed.bonusSpeed);
        BonusAgiValueText.GetComponent<Text>().text = ClearIfZero(characterBeingDisplayed.bonusAgility);
        BonusIntValueText.GetComponent<Text>().text = ClearIfZero(characterBeingDisplayed.bonusIntelligence);
        LifeText.GetComponent<Text>().text = characterBeingDisplayed.currentHealth.ToString() + "/" + characterBeingDisplayed.maxHealth.ToString();
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
