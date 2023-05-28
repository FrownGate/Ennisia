using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartConversationTemp : MonoBehaviour
{
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        FindAnyObjectByType<DialogueManager>().StartDialogue(dialogue);
    }
}
