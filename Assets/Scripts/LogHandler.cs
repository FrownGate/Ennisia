using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogHandler : MonoBehaviour
{
    public static LogHandler Instance { get; private set; }

    [SerializeField] Canvas _canvas;
    [SerializeField] private TextMeshProUGUI _promptText;
    [SerializeField] private ScrollRect _scrollRect;
    
    public CanvasGroup MessageBoxCanvasGroup;
    private bool _isMessageBoxVisible = true;
    private readonly float _offset = -1.5f;
    
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
    
    private void LogMessageReceived(string logString, string stackTrace, LogType type)
    {
        string color = null;

        switch (type)
        {
            case LogType.Error:
            case LogType.Exception:
            case LogType.Assert:
                color = "red";
                break;
            case LogType.Warning:
                color = "#ff8a00";
                break;
            case LogType.Log:
                color = "white";
                break;
        }

        EnqueueMessage($"\n <color={color}>[{type}] {logString}</color>");
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