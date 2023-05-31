using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private TMP_Text _nameText;
    private TMP_Text _dialogueText;
    private Queue<string> _names;
    private Queue<string> _dialogues;

    private void Awake()
    {
        // add events, events need an int for the CSVid
        StartDialogue(1); //testing only
    }

    public void StartDialogue(int CSVid)
    {
        Dialogue dialogue = new(CSVid);

        _dialogues = new Queue<string>();
        _names = new Queue<string>();

        //_names.Clear();

        //_dialogues.Clear();

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
        _nameText.text = name;

        string dialogue = _dialogues.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeDialogue(dialogue));
    }

    IEnumerator TypeDialogue(string dialogue)
    {
        _dialogueText.text = "";

        foreach (char letter in dialogue.ToCharArray())
        {
            _dialogueText.text += letter;
            yield return null;
        }
    }

    public void EndDialogue()
    {
        // add end state
    }
}
