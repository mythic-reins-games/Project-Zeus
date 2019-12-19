#pragma warning disable 0649

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIPanel : MonoBehaviour, IGameSignalOneObjectListener
{

    [SerializeField] List<Image> actionPointImages;
    [SerializeField] Slider concentrationSlider;
    [SerializeField] Image concentrationImage;

    [SerializeField] private GameSignalOneObject gameSignal;
    private OneObjectEvent gameSignalEvent;

    int numPoints = 0;

    Text backstabValueText = null;
    Text dodgeValueText = null;
    Text damageValueText = null;
    Text accValueText = null;
    Text critValueText = null;
    Text staminaValueText = null;
    Text lifeValueText = null;
    Text nameValueText = null;
    Button button0 = null;
    Button button1 = null;
    Button button2 = null;
    Button button3 = null;

    private void OnEnable()
    {
        gameSignal.RegisterListener(this);
    }

    private void OnDisable()
    {
        gameSignal.DeRegisterListener(this);
    }

    public void OnGameSignalRaised(object value)
    {
        gameSignalEvent.Invoke(value);
    }

    void Start()
    {
        dodgeValueText = GameObject.Find("DodgeValueText").GetComponent<Text>();
        backstabValueText = GameObject.Find("BackstabValueText").GetComponent<Text>();
        damageValueText = GameObject.Find("DamageValueText").GetComponent<Text>();
        accValueText = GameObject.Find("AccValueText").GetComponent<Text>();
        critValueText = GameObject.Find("CritValueText").GetComponent<Text>();
        staminaValueText = GameObject.Find("StaminaValueText").GetComponent<Text>();
        lifeValueText = GameObject.Find("LifeValueText").GetComponent<Text>();
        nameValueText = GameObject.Find("NameValueText").GetComponent<Text>();
        button0 = GameObject.Find("Button0").GetComponent<Button>();
        button1 = GameObject.Find("Button1").GetComponent<Button>();
        button2 = GameObject.Find("Button2").GetComponent<Button>();
        button3 = GameObject.Find("Button3").GetComponent<Button>();

        gameSignalEvent = new OneObjectEvent();
        gameSignalEvent.AddListener(SetConcentrationSlider);
    }

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        if (GetActivePlayerController() != null) {
            string[] names = GetActivePlayerController().AbilityNames();
            if (names.Length > 0) button0.GetComponentInChildren<Text>().text = names[0];
            if (names.Length > 1) button1.GetComponentInChildren<Text>().text = names[1];
            if (names.Length > 2) button2.GetComponentInChildren<Text>().text = names[2];
            if (names.Length > 3) button3.GetComponentInChildren<Text>().text = names[3];
        }
    }

    public void DisplayStats(CreatureMechanics unit)
    {
        dodgeValueText.text = unit.DodgeChance() + " %";
        backstabValueText.text = unit.BonusRearDamageMin() + "-" + unit.BonusRearDamageMax();
        damageValueText.text = unit.MinDamage() + "-" + unit.MaxDamage();
        accValueText.text = unit.HitChance() + " %";
        critValueText.text = unit.CritChance() + " %";
        lifeValueText.text = unit.LifeString();
        staminaValueText.text = unit.StaminaString();
        nameValueText.text = unit.displayName;
    }

    public void ClearActionPoints()
    {
        foreach (Image im in actionPointImages)
        {
            im.color = new Color32(0, 0, 0, 255);
        }
    }

    private PlayerController GetActivePlayerController()
    {
        PlayerController[] controllers = Object.FindObjectsOfType<PlayerController>();
        foreach (PlayerController controller in controllers)
        {
            if (controller.isTurn && !controller.isActing) return controller;
        }
        return null;
    }

    public void ButtonClick(int buttonId)
    {
        PlayerController controller = GetActivePlayerController();
        if (controller == null) return;
        controller.AbilityButtonClick(buttonId);
    }

    public void EndTurnButtonClick()
    {
        PlayerController controller = GetActivePlayerController();
        if (controller == null) return;
        controller.EndTurnButtonClick();
    }

    public void SetActionPoints(int amount)
    {
        if (amount > actionPointImages.Count)
        {
            Debug.LogWarning("Warning! Not enough action point images in GUI.");
            return;
        }
        for (numPoints = 0; numPoints < amount; numPoints++)
        {
            actionPointImages[numPoints].color = new Color32(0, 255, 0, 255);
        }
    }

    public void SpendActionPoints(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            numPoints -= 1;
            actionPointImages[numPoints].color = new Color32(255, 0, 0, 255);
        }
    }
    
    public void SetConcentrationSlider(object value)
    {
        float? fValue = value as float?;
        if (concentrationSlider)
        {
            concentrationSlider.value = fValue.HasValue ? fValue.Value : 0f;
        }
        if (concentrationImage)
        {
            concentrationImage.material.SetFloat("_Value", fValue.HasValue ? fValue.Value : 0f);
        }
    }
}
