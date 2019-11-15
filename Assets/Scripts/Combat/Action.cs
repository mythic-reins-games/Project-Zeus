using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    protected enum Phase
    {
        NONE,
        MOVING,
        CASTING,
        ATTACKING,
    };

    public enum TargetType
    {
        NONE,
        SELF_ONLY,
        CHARGE,
        MELEE,
    };

    protected Animator anim;

    protected int spentActionPoints = 0;

    protected bool inProgress = false;

    private CombatController combatController;
    protected CreatureMechanics mechanics;

    virtual public int CONCENTRATION_COST { get { return 0; } }
    // Lets UI/AI know to ignore the move if fewer than this many AP.
    virtual public int MIN_AP_COST { get { return 0; } }
    // Target type for special moves, lets UI/AI know when it can use special moves.
    virtual public TargetType TARGET_TYPE { get { return TargetType.NONE; } }

    protected Phase currentPhase = Phase.NONE;

    // Start is called before the first frame update
    virtual protected void Start()
    {
        combatController = GetComponent<CombatController>();
        anim = GetComponentInChildren<Animator>();
        mechanics = GetComponent<CreatureMechanics>();
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
        mechanics.currentConcentration -= CONCENTRATION_COST;
        combatController.EndAction(spentActionPoints);
        inProgress = false;
    }
}
