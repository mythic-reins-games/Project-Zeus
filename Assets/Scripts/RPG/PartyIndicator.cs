using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyIndicator : MonoBehaviour
{

    private GameObject charIndicatorPrefab;

    void ClearCharacterIndicators()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    void Start()
    {
        charIndicatorPrefab = (GameObject)Resources.Load("Prefabs/CharacterIndicator", typeof(GameObject));
    }

    // Update is called once per frame
    void Update()
    {
        ClearCharacterIndicators();
        float xPos = 0f;
        foreach (CharacterSheet c in PlayerParty.partyMembers)
        {
            Vector3 location = new Vector3(transform.position.x + xPos, transform.position.y, 0f);
            GameObject combatant = GameObject.Instantiate(charIndicatorPrefab, location, Quaternion.identity, transform) as GameObject;
            combatant.transform.Find("Text").GetComponent<Text>().text = c.name;
            xPos += 100f;
        }
    }
}
