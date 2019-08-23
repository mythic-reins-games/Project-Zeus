using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    private ConversationUI _conversationUI;
    public ConversationUI ConversationUI { get { return _conversationUI; } }

    void Start()
    {
        this._conversationUI = this.GetComponentInChildren<ConversationUI>();
    }
}
