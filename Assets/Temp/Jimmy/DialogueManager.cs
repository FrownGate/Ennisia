using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DialogueManager : MonoBehaviour
{

    private TMP_Text nameText;
    private TMP_Text dialogueText;
    private Queue<string> _names;
    private Queue<string> _dialogues;

    private void Awake()
    {
        // add events, events need an int for the CSVid
    }

    public void StartDialogue(int CSVid)
    {
        Dialogue dialogue = new();

        dialogue.readCSVFile(CSVid);

        _dialogues = new Queue<string>();
        _names = new Queue<string>();

        _names.Clear();

        _dialogues.Clear();

        foreach (var item in dialogue.name)
        {
            _names.Enqueue(item);
        }

        foreach (var item in dialogue.dialogues)
        {
            _dialogues.Enqueue(item);
        }

        DisplayNextDialogue();

    }

    public void DisplayNextDialogue()
    {
        if (_names.Count == 0)
        {
            EndDialogue();
            return;
        }

        if (_dialogues.Count == 0)
        {
            EndDialogue();
            return;
        }

        string name = _names.Dequeue();
        nameText.text = name;
        Debug.Log(name);

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
