using System.Collections.Generic;
using UnityEngine;

public class SortCombatantDescendant : IComparer<GameObject>
{
    public int Compare(GameObject x, GameObject y)
    {
        var xMechanic = x?.GetComponent<CreatureMechanics>();
        var yMechanic = y?.GetComponent<CreatureMechanics>();
        return yMechanic.Speed.CompareTo(xMechanic.Speed);
    }
}