using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyIndicator : MonoBehaviour
{

    private GameObject charIndicatorPrefab;

    protected void ClearCharacterIndicators()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    virtual protected bool isPC()
    {
        return true;
    }

    protected void UpdateSingleSheet(CharacterSheet c, float xPos)
    {
        Vector3 location = new Vector3(transform.position.x + xPos, transform.position.y, 0f);
        GameObject indicator = GameObject.Instantiate(charIndicatorPrefab, location, Quaternion.identity, transform) as GameObject;
        indicator.transform.Find("Text").GetComponent<Text>().text = c.name;
        indicator.GetComponent<CharacterIndicatorClick>().toDisplay = c;
        indicator.GetComponent<CharacterIndicatorClick>().isPC = isPC();
    }

    void Start()
    {
        charIndicatorPrefab = (GameObject)Resources.Load("Prefabs/CharacterIndicator", typeof(GameObject));
        PartyCompositionChanged();
    }

    virtual public void PartyCompositionChanged()
    {
        ClearCharacterIndicators();
        float xPos = 0f;
        foreach (CharacterSheet c in PlayerParty.partyMembers)
        {
            UpdateSingleSheet(c, xPos);
            xPos += 100f;
        }
    }
}
