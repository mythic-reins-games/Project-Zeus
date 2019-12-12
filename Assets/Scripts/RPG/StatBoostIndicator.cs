using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatBoostIndicator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject statIndicatorPrefab = (GameObject)Resources.Load("Prefabs/StatIndicator", typeof(GameObject));
        float xPos = 0f;
        foreach (CharacterSheet c in PlayerParty.partyMembers)
        {
            Vector3 location = new Vector3(transform.position.x + xPos, transform.position.y, 0f);
            GameObject combatant = GameObject.Instantiate(statIndicatorPrefab, location, Quaternion.identity, transform) as GameObject;
            combatant.transform.Find("Text").GetComponent<Text>().text = c.boostStatText;
            xPos += 100f;
        }
    }
}
