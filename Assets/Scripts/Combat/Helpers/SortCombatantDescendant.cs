using System.Collections.Generic;
using UnityEngine;

public class SortCombatantDescendant : IComparer<GameObject>
{
    //TODO: Using GameObject to make it compatible with TurnManager
    public int Compare(GameObject x, GameObject y)
    {
        var xMechanic = x?.GetComponent<CreatureMechanics>();
        var yMechanic = y?.GetComponent<CreatureMechanics>();
        return yMechanic.Speed.CompareTo(xMechanic.Speed);
    }
}