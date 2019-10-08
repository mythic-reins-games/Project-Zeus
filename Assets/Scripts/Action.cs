using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{

    protected int spentActionPoints = 0;

    protected bool inProgress = false;

    private CombatController combatController;

    // Start is called before the first frame update
    virtual protected void Start()
    {
        combatController = GetComponent<CombatController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    virtual public void BeginAction(Tile targetTile)
    {
        // Re-initialize the number of spent action points to 0.
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
