using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    private Queue<string> _dialogues;

    void Start()
    {
        _dialogues = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        nameText.text = dialogue.name; //changes with csv

        _dialogues.Clear();

        foreach (var item in dialogue.dialogues)
        {
            _dialogues.Enqueue(item);
        }

        DisplayNextDialogue();

    }

    public void DisplayNextDialogue()
    {

        if (_dialogues.Count == 0)
        {
            EndDialogue();
            return;
        }

        string dialogue = _dialogues.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeDialogue(dialogue));
    }

    IEnumerator TypeDialogue(string dialogue)
    {
        dialogueText.text = "";
        foreach (char letter in dialogue.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    public void EndDialogue()
    {
        // add end state
    }
}
