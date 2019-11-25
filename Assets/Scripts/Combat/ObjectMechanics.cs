using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMechanics : MonoBehaviour
{

    protected int maxHealth = 1;
    protected int currentHealth = 1;

    public bool dead = false;

    [SerializeField] protected GameObject healthBar;
    protected IndicatorBar healthBarScript;

    public List<StatusEffect> statusEffects = new List<StatusEffect>();

    virtual public bool canBeBackstabbed { get { return false; } }

    private Animator anim;

    // Start is called before the first frame update
    virtual protected void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    virtual public int DodgeChance()
    {
        return 0;
    }

    public string LifeString()
    {
        return currentHealth + "/" + maxHealth;
    }

    protected float PercentHealth()
    {
        return (float)currentHealth / (float)maxHealth;
    }

    public virtual void ReceiveDamage(int amount)
    {
        DisplayPopupAfterDelay(0.2f, amount + " damage");
        currentHealth -= (amount);
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            Animate("IsGettingDamaged");
        }
    }

    public void UnsetStatusAnimation(string animName)
    {
        if (animName != null)
        {
            anim.SetBool(animName, false);
        }
    }

    protected void SetStatusAnim(string animName)
    {
        if (animName != null)
        {
            anim.SetBool(animName, true);
        }
    }

    private TileBlockerController GetController()
    {
        if (GetComponent<PlayerController>() != null) return GetComponent<PlayerController>();
        if (GetComponent<EnemyController>() != null) return GetComponent<EnemyController>();
        if (GetComponent<TileBlockerController>() != null) return GetComponent<TileBlockerController>();
        return null;
    }

    protected void Die()
    {
        dead = true;
        GetController().UnassignCurrentTile();
        Animate("IsDying");
        Destroy(gameObject, 0.9f);
    }

    public void DisplayPopup(string text)
    {
        PopupTextController.CreatePopupText(text, transform);
    }

    private IEnumerator RunPopupDelay(float time, string text)
    {
        yield return new WaitForSeconds(time);
        DisplayPopup(text);
        yield break;
    }

    // Public wrapper for the enumerator.
    public void DisplayPopupAfterDelay(float time, string text)
    {
        StartCoroutine(RunPopupDelay(time, text));
    }

    protected IEnumerator ClearAnimationsAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);
        ClearAnimations();
        yield break;
    }

    private void ClearAnimations()
    {
        anim.SetBool("IsAttacking", false);
        anim.SetBool("IsDodging", false);
        anim.SetBool("IsGettingDamaged", false);
        anim.SetBool("IsCastingSpell", false);
    }

    virtual public void Animate(string animName)
    {
        if (anim == null) return;
        anim.SetBool(animName, true);
        StartCoroutine(ClearAnimationsAfterDelay(0.5f));
    }
}
