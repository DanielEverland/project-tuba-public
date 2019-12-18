using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DateTime = System.DateTime;

public class DebugConsole : MonoBehaviour
{
    [SerializeField]
    private TMP_Text textPrefab = default;
    [SerializeField]
    private GameObject console = default;
    [SerializeField]
    private Transform content = default;

    private static readonly Dictionary<LogType, Color> Colors = new Dictionary<LogType, Color>()
    {
        { LogType.Assert, Color.red },
        { LogType.Error, Color.red },
        { LogType.Exception, Color.red },
        { LogType.Warning, Color.yellow },
        { LogType.Log, Color.white },
    };

    private Dictionary<int, LogEntry> entries = new Dictionary<int, LogEntry>();
    private HashSet<LogType> acceptedLogTypes = new HashSet<LogType>()
    {
        LogType.Assert,
        LogType.Error,
        LogType.Exception,
        LogType.Warning,
    };

    public void CopyToClipboard()
    {
        string allEntries = "";

        foreach (var item in entries.Values)
        {
            allEntries += $"{item.Type}: {item.ToString()}\n";
        }

        GUIUtility.systemCopyBuffer = allEntries;
    }

    private void Awake()
    {
        if(!Application.isEditor && Debug.isDebugBuild)
            Application.logMessageReceived += LogMessage;

        console.SetActive(false);
    }
    private void PollShowConsole()
    {
        if (Input.GetKeyDown(KeyCode.F9))
            ToggleConsole();
    }
    private void LogMessage(string message, string stackTrace, LogType type)
    {
        if (!ShouldLogType(type))
            return;

        ShowConsole();
        PollEntries(message, stackTrace, type);     
    }
    private void PollEntries(string message, string stackTrace, LogType type)
    {
        int id = stackTrace.GetHashCode();

        if (entries.ContainsKey(id))
        {
            UpdateExisting(id);
        }
        else
        {
            CreateNewEntry(id, message, stackTrace, type);
        }
    }
    private void ToggleConsole()
    {
        console.SetActive(!console.activeSelf);
    }
    private void ShowConsole()
    {
        console.SetActive(true);
    }
    private bool ShouldLogType(LogType type)
    {
        return acceptedLogTypes.Contains(type);
    }
    private void UpdateExisting(int id)
    {
        entries[id].WasRaised();
    }
    private void CreateNewEntry(int id, string condition, string stackTrace, LogType type)
    {
        LogEntry entry = new LogEntry(condition, stackTrace, type, content, textPrefab);
        entries.Add(id, entry);
    }
    
    private class LogEntry
    {
        public LogEntry(string message, string stackTrace, LogType type, Transform textElementParent, TMP_Text prefab)
        {
            this.message = message;
            this.stackTrace = stackTrace;
            this.type = type;

            time = DateTime.Now;
            frameCount = Time.frameCount;

            textElement = CreateTextElement(textElementParent, prefab);
            UpdateText();
        }

        public LogType Type => type;

        private readonly string message;
        private readonly string stackTrace;
        private readonly LogType type;
        private readonly TMP_Text textElement;
        private readonly DateTime time;
        private readonly int frameCount;
        
        private bool IsOngoing => timesRaised > 1;

        private int timesRaised = 1;
        
        public void WasRaised()
        {
            timesRaised++;

            UpdateText();
        }
        public void UpdateText()
        {
            textElement.text = ToString();
        }
        private TMP_Text CreateTextElement(Transform parent, TMP_Text prefab)
        {
            TMP_Text instance = Instantiate(prefab);
            instance.transform.SetParent(parent);
            instance.color = GetColor(type);

            return instance;
        }
        private static Color GetColor(LogType logType)
        {
            if (!Colors.ContainsKey(logType))
                throw new System.NullReferenceException($"{logType} is not declared!");

            return Colors[logType];
        }
        public override string ToString()
        {
            if (IsOngoing)
            {
                return $"{message}\n[{time}] [ONGOING({timesRaised})] FrameCount: {frameCount}\n{stackTrace}";
            }
            else
            {
                return $"{message}\n[{time}] FrameCount: {frameCount}\n{stackTrace}";
            }
        }
    }

    private class LogTypeList : ReorderableArray<LogType> { }
}
