using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    public static event Action OnDialogueEnd;

    [SerializeField] private GameObject _dialogueBox;
    [SerializeField] private GameObject _spriteBox;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _dialogueText;
    private Queue<string> _names;
    private Queue<string> _dialogues;
    private Queue<Sprite> _sprites;
    private Sprite[] _allSprites;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
            _allSprites = Resources.LoadAll<Sprite>($"Textures/CharacterSplash");

        }
    }
    private void Start()
    {
        StartDialogue(1); //testing only
    }

    public void StartDialogue(int CSVid)
    {
        Dialogue dialogue = new(CSVid);

        _dialogues = new Queue<string>();
        _names = new Queue<string>();
        _sprites = new Queue<Sprite>();

        _dialogueBox.SetActive(true);

        //_names.Clear();

        //_dialogues.Clear();

        for (int i = 0; i < dialogue.name.Count; i++)
        {
            if (PlayFabManager.Instance.Player.Name == null) { Debug.LogError("Player name is null"); }

            if (dialogue.name[i] == "Me")
            {
                dialogue.name[i] = PlayFabManager.Instance.Player.Name;
            }
            _names.Enqueue(dialogue.name[i]);
        }

        foreach (var item in dialogue.dialogues)
        {
            _dialogues.Enqueue(item);
        }


        foreach (var item in _allSprites)
        {
            if (_names.Peek() == PlayFabManager.Instance.Player.Name)
            {
                if (PlayFabManager.Instance.Account.Gender == 1)
                {
                    _sprites.Enqueue(_allSprites[0]);
                }
                else { _sprites.Enqueue(_allSprites[1]); }
            }

            if (item.name.Contains(_names.Peek()))
            {
                _sprites.Enqueue(item);
            }
        }

        DisplayNextDialogue();
    }

    public void DisplayNextDialogue()
    {
        if (_names.Count == 0 || _dialogues.Count == 0)
        {
            EndDialogue();
            return;
        }

        string name = _names.Dequeue();
        _nameText.text = name;

        Sprite sprite = _sprites.Dequeue();
        _spriteBox.GetComponent<SpriteRenderer>().sprite = sprite;

        string dialogue = _dialogues.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeDialogue(dialogue));
    }

    IEnumerator TypeDialogue(string dialogue)
    {
        _dialogueText.text = "";

        foreach (char letter in dialogue.ToCharArray())
        {
            yield return new WaitForSeconds(0.05f);
            _dialogueText.text += letter;
            yield return null;
        }
    }

    public void EndDialogue()
    {
        _dialogueBox.SetActive(false);
        OnDialogueEnd?.Invoke();
    }
}