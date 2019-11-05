#pragma warning disable 0649

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIPanel : MonoBehaviour, IGameSignalOneObjectListener
{

    [SerializeField] List<Image> actionPointImages;
    [SerializeField] Slider concentrationSlider;

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

        gameSignalEvent = new OneObjectEvent();
        gameSignalEvent.AddListener(SetConcentrationSlider);
    }

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    public void DisplayStats(CreatureStats unit)
    {
        dodgeValueText.text = unit.DodgeChance() + " %";
        backstabValueText.text = unit.BonusRearDamageMin() + "-" + unit.BonusRearDamageMax();
        damageValueText.text = unit.MinDamage() + "-" + unit.MaxDamage();
        accValueText.text = unit.HitChance() + " %";
        critValueText.text = unit.CritChance() + " %";
        lifeValueText.text = unit.LifeString();
        staminaValueText.text = unit.StaminaString();
        nameValueText.text = unit.name;
    }

    public void ClearActionPoints()
    {
        foreach (Image im in actionPointImages)
        {
            im.color = new Color32(0, 0, 0, 255);
        }
    }

    public void EndTurnButtonClick()
    {
        PlayerController[] controllers = Object.FindObjectsOfType<PlayerController>();
        foreach (PlayerController controller in controllers)
        {
            controller.EndTurnButtonClick();
        }
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
        concentrationSlider.value = fValue.HasValue ? fValue.Value : 0f;
    }
}
