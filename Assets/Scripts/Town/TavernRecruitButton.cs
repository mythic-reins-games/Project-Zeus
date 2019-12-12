using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernRecruitButton : MonoBehaviour
{
    [SerializeField]
    CharacterSheet.CharacterClass characterClass;

    public void Recruit()
    {
        if (PlayerParty.gold >= 15 && PlayerParty.partyMembers.Count < Constants.MAX_PARTY_SIZE)
        {
            PlayerParty.AddToParty(characterClass);
            PlayerParty.gold -= 15;
            PartyIndicator toUpdate = (PartyIndicator)FindObjectOfType(typeof(PartyIndicator));
            toUpdate.PartyCompositionChanged();
        }
    }
}
