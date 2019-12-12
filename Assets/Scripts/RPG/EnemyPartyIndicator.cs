using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPartyIndicator : PartyIndicator
{
    override protected bool isPC()
    {
        return false;
    }

    override public void PartyCompositionChanged()
    {
        ClearCharacterIndicators();
        float xPos = 0f;
        foreach (CharacterSheet c in EnemyParty.partyMembers)
        {
            UpdateSingleSheet(c, xPos);
            xPos += 100f;
        }
    }
}
