
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
        }
        private void Update()
        {
            LogException(new Exception(GetLogMessage()),this);
        }
        
        public void LogFormat(LogType logType, Object context, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void LogException(Exception exception, Object context)
        {
            throw new NotImplementedException();
        }

        public string GetLogMessage()
        {
            string msg = "";
            
            return msg;
        }
    }
