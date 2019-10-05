using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{

    protected int spentActionPoints = 0;

    protected bool inProgress = false;

    protected CombatController combatController;

    // Start is called before the first frame update
    void Start()
    {
        combatController = GetComponent<CombatController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void BeginAction(Tile targetTile)
    {
        spentActionPoints = 0;
        inProgress = true;
        combatController.BeginAction();
    }

    protected void EndAction()
    {
        inProgress = false;
        combatController.EndAction(spentActionPoints);
    }
}
