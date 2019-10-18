using UnityEngine;
using UnityEngine.UI;

public class PopupText : MonoBehaviour
{
    // animator is from the child component PopupText
    [SerializeField] private Animator animator;
    private Text textComponent;

    void Awake()
    {
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, clipInfo[0].clip.length);
        textComponent = animator.GetComponent<Text>();
    }

    public void SetText(string text)
    {
        textComponent.text = text;
    }
}
