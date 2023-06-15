
    using System;
    using TMPro;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public class LogHandler : MonoBehaviour, ILogHandler
    {
        [SerializeField] TextMeshProUGUI _promptText;
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
            }
        }

        public void LogException(Exception exception, Object context)
        {
            _promptText.text += "\n [Exception] " + exception.Message;
        }
        
    }
