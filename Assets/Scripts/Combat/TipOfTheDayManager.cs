using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipOfTheDayManager : MonoBehaviour
{

    System.Random rng;

    // Start is called before the first frame update
    void Start()
    {
        rng = new System.Random();
        GameObject.Find("TipOfTheDayText").GetComponent<Text>().text = GetRandomTipOfTheDay();
    }

    private string GetRandomTipOfTheDay()
    {
        switch (rng.Next(1, 15))
        {
            case 1:
                return "You can press the keyboard keys 1-4 to activate your special powers";
            case 2:
                return "You can use the q and e keys to rotate the camera";
            case 3:
                return "Attacks from high ground inflict +10% damage, or +20% if you are two levels above";
            case 4:
                return "Reach units like the Myrmadon can attack from two tiles away and have an expanded zone of control";
            case 5:
                return "It costs an extra action point to leave an enemy unit's zone of control";
            case 6:
                return "Attacking a unit from behind inflicts extra backstab damage";
            case 7:
                return "Attacking blinded or knocked down foe inflicts extra backstab damage";
            case 8:
                return "Units have both stamina (orange bar) and health (red bar): stamina heals at the end of combat, but health damage persists";
            case 9:
                return "Units that take health damage have all of their statistics progressively weakened";
            case 10:
                return "Normally, units only take health damage when they run out of stamina, but poison inflicts DOT directly to health";
            case 11:
                return "Petrified units can't move or dodge, but also can't be backstabbed and resist 50% of incoming damage";
            case 12:
                return "Frozen units can't move or dodge";
            case 13:
                return "Attack enemy units or destructible objects to gain concentration for special attacks";
            case 14:
                return "The archer has a powerful ranged attack, but exerts no zone of control";
        }
        return "No more tips of the day!";
    }

    public void Close()
    {
        Destroy(transform.parent.gameObject);
    }
}
