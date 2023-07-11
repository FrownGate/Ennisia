using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LogHandler : MonoBehaviour, IPointerClickHandler
{
    public static LogHandler Instance { get; private set; }

    [SerializeField] Canvas _canvas;
    [SerializeField] private TextMeshProUGUI _promptText;
    [SerializeField] private ScrollRect _scrollRect;
    
    public CanvasGroup MessageBoxCanvasGroup;
    private bool _isMessageBoxVisible = true;
    private float _offset => -1.5f;

    //private bool _isMessageBoxVisible = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        Application.logMessageReceived += LogMessageReceived;
    }

    private void OnDestroy()
    {
        Application.logMessageReceived -= LogMessageReceived;
    }

    private void Update()
    {
        InputSpaceToHideMessageBox();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        ToggleMessageBox();
    }
    

    private void LogMessageReceived(string logString, string stackTrace, LogType type)
    {
        switch (type)
        {
            case LogType.Error:
                EnqueueMessage($"\n <color=red>[{type}] {logString}</color>");
                break;
            case LogType.Exception:
            case LogType.Assert:
                EnqueueMessage($"\n <color=red>[{type}] {logString}</color>");
                break;
            case LogType.Warning:
                EnqueueMessage($"\n <color=#ff8a00>[{type}] {logString}</color>");
                break;
            case LogType.Log:
                EnqueueMessage($"\n <color=white>[{type}] {logString}</color>");
                break;
        }
    }

    private void EnqueueMessage(string message)
    {
        _promptText.text += message;
        _scrollRect.normalizedPosition = new Vector2(-0.1f, _offset);
    }

    private void InputSpaceToHideMessageBox()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleMessageBox();
        }
    }
    
    public void ToggleMessageBox()
    {
        _isMessageBoxVisible = !_isMessageBoxVisible;
        _canvas.gameObject.SetActive(_isMessageBoxVisible);
    }
}