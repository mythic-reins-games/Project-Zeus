using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIPanel : MonoBehaviour
{

    [SerializeField]
    List<Image> actionPointImages;

    int numPoints = 0;

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
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
    
}
