using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is a base class that all other types of actions inherit from.
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

    // 0 means ready to cast, gets set to COOLDOWN when special ability used; decremented by 1 each turn.
    private int cooldownProgress = 0;
    protected Animator anim;
    protected int spentActionPoints = 0;
    protected bool inProgress = false;
    private CombatController combatController;
    protected CreatureMechanics mechanics;

    virtual public int COOLDOWN { get { return 0; } }
    virtual public int CONCENTRATION_COST { get { return 0; } }

    // Lets UI/AI know to ignore the move if fewer than this many AP.
    virtual public int MIN_AP_COST { get { return 0; } }
    // Target type for special moves, lets UI/AI know when it can use special moves.
    virtual public TargetType TARGET_TYPE { get { return TargetType.NONE; } }

    protected Phase currentPhase = Phase.NONE;

    protected IEnumerator EndActionAfterDelay(float fDuration)
    {
        yield return new WaitForSeconds(fDuration);
        currentPhase = Phase.NONE;
        EndAction();
        yield break;
    }

    public void AdvanceCooldown()
    {
        if (cooldownProgress == 0) return;
        cooldownProgress -= 1;
    }

    public bool IsCoolingDown()
    {
        return cooldownProgress > 0;
    }

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
        cooldownProgress = COOLDOWN;
        combatController.BeginAction();
    }

    protected void EndAction()
    {
        mechanics.currentConcentration -= CONCENTRATION_COST;
        combatController.EndAction(spentActionPoints);
        inProgress = false;
    }
}
