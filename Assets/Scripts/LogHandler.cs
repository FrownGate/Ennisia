using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogHandler : MonoBehaviour
{
    public static LogHandler Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI _promptText;
    [SerializeField] private ScrollRect _scrollView;
    [SerializeField] private int _maxLogMessages = 22;
    [SerializeField] private Canvas _canvas;

    private readonly Queue<string> _logMessagesQueue = new();
    private bool _isMessageBoxVisible = true;

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

        //Debug.unityLogger.logHandler = this;
        Application.logMessageReceived += LogMessageReceived;
        _canvas.worldCamera = Camera.main;
    }

    private void OnDestroy()
    {
        Application.logMessageReceived -= LogMessageReceived;
    }

    private void Update()
    {
        InputSpaceToHideMessageBox();
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
        if (_logMessagesQueue.Count >= _maxLogMessages)
        {
            _logMessagesQueue.Dequeue();
        }
        _logMessagesQueue.Enqueue(message);
        _promptText.text = string.Join("", _logMessagesQueue.ToArray());
    }

    private void InputSpaceToHideMessageBox()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isMessageBoxVisible = !_isMessageBoxVisible;
            _promptText.gameObject.SetActive(_isMessageBoxVisible);
            _scrollView.gameObject.SetActive(_isMessageBoxVisible);
        }
    }
}