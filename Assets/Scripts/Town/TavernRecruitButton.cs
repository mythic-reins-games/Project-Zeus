using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernRecruitButton : MonoBehaviour
{
    [SerializeField]
    CharacterSheet.CharacterClass characterClass;

    public void Recruit()
    {
        if (PlayerParty.gold >= 15 && PlayerParty.partyMembers.Count < 3)
        {
            PlayerParty.AddToParty(characterClass);
            PlayerParty.gold -= 15;
        }
    }
}
