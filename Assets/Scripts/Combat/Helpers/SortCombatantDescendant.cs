using System.Collections.Generic;
using UnityEngine;

public class SortCombatantDescendant : IComparer<CombatController>
{
    public int Compare(CombatController x, CombatController y)
    {
        var xMechanic = x?.GetComponent<CreatureMechanics>();
        var yMechanic = y?.GetComponent<CreatureMechanics>();
        return yMechanic.Speed.CompareTo(xMechanic.Speed);
    }
}