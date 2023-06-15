
    using System;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public class LogHandler : MonoBehaviour, ILogHandler
    {
        [SerializeField] TextMeshProUGUI _promptText;
        [SerializeField] int maxLogMessages = 100;
        private Queue<string> _logMessagesQueue = new Queue<string>();
        public static LogHandler Instance { get; private set; }
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
            Debug.unityLogger.logHandler = this;
            Application.logMessageReceived += LogMessageReceived;
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= LogMessageReceived;
        }
        
        public void LogFormat(LogType logType, Object context, string format, params object[] args)
        {
            if (logType == LogType.Error)
            {
                string message = String.Format(format, args);
                _promptText.text += "\n [Error] " + message;
            }
            else if (logType == LogType.Exception)
            {
                string message = String.Format(format, args);
                _promptText.text += "\n [Exception] " + message;
            }else if (logType == LogType.Log)
            {
                string message = String.Format(format, args);
                _promptText.text += "\n [Exception] " + message;
            }
        }

        public void LogException(Exception exception, Object context)
        {
            _promptText.text += "\n [Exception] " + exception.Message;
        }
        
        private void LogMessageReceived(string logString, string stackTrace, LogType type)
        {
            string message = "";
            switch (type)
            {
                case LogType.Error:
                    message = "\n [Error] " + logString;
                    break;
                case LogType.Exception:
                    message = "\n [Exception] " + logString;
                    break;
                default:
                    message = "\n" + logString;
                    break;
            }
            EnqueueMessage(message);
        }
        
        private void EnqueueMessage(string message)
        {
            if(_logMessagesQueue.Count >= maxLogMessages)
            {
                _logMessagesQueue.Dequeue();
            }

            _logMessagesQueue.Enqueue(message);

            _promptText.text = string.Join("\n", _logMessagesQueue.ToArray());
        }
        
    }
