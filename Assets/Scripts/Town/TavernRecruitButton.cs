using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernRecruitButton : MonoBehaviour
{
    [SerializeField]
    CharacterSheet.CharacterClass characterClass;

    public void Recruit()
    {
        if (PlayerParty.gold < 15)
        {
            NotificationPopupSystem.PopupText("Not enough gold");
            return;
        }
        if (PlayerParty.partyMembers.Count >= Constants.MAX_PARTY_SIZE)
        {
            NotificationPopupSystem.PopupText("Party already full");
            return;
        }
        PlayerParty.AddToParty(characterClass);
        PlayerParty.gold -= 15;
        PartyIndicator toUpdate = (PartyIndicator)FindObjectOfType(typeof(PartyIndicator));
        toUpdate.PartyCompositionChanged();
    }
}
