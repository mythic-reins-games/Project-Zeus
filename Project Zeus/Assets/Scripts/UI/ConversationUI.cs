using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationUI : MonoBehaviour
{
    private Conversation currentConversation;

    public void ClearConversation()
    {
        this.currentConversation = null;
    }

    public void SetConversation(Conversation conversation)
    {
        this.currentConversation = conversation;
    }
}
