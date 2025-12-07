using System;
using System.IO;
using UnityEngine;

public class ErrorLogger : MonoBehaviour {
    private static ErrorLogger instance;
    private string logFilePath;
    private const string LOG_FOLDER = "Logs";
    private const string LOG_FILE = "error_log.txt";

    private void Awake() {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeLogger();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeLogger() {
        try
        {
            // Create logs folder if it doesn't exist
            string logFolder = Path.Combine(Application.persistentDataPath, LOG_FOLDER);

            if (!Directory.Exists(logFolder))
            {
                Directory.CreateDirectory(logFolder);
            }

            logFilePath = Path.Combine(logFolder, LOG_FILE);

            // Write initial header if file doesn't exist
            if (!File.Exists(logFilePath))
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string header = "=== ERROR LOG STARTED ===" + "\n" + "Date: " + timestamp + "\n\n";
                File.WriteAllText(logFilePath, header);
            }

            Debug.Log("Error logger initialized at: " + logFilePath);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to initialize error logger: " + e.Message);
        }
    }

    /// <summary>
    /// Logs an error message to the file
    /// </summary>
    public static void LogError(string context, string errorMessage) {
        if (instance != null)
        {
            instance.WriteToFile(context, errorMessage, "ERROR");
        }
    }

    /// <summary>
    /// Logs a warning message to the file
    /// </summary>
    public static void LogWarning(string context, string warningMessage) {
        if (instance != null)
        {
            instance.WriteToFile(context, warningMessage, "WARNING");
        }
    }

    /// <summary>
    /// Logs an exception to the file
    /// </summary>
    public static void LogException(string context, Exception exception) {
        if (instance != null)
        {
            string exceptionDetails = exception.Message + "\nStack Trace: " + exception.StackTrace;
            instance.WriteToFile(context, exceptionDetails, "EXCEPTION");
        }
    }

    private void WriteToFile(string context, string message, string level) {
        try
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string separator = new string('-', 80);
            string logEntry = "[" + timestamp + "] [" + level + "] [" + context + "]\n" + message + "\n" + separator + "\n\n";

            File.AppendAllText(logFilePath, logEntry);

            // Also log to Unity console
            if (level == "ERROR" || level == "EXCEPTION")
            {
                Debug.LogError("[" + context + "] " + message);
            }
            else if (level == "WARNING")
            {
                Debug.LogWarning("[" + context + "] " + message);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to write to error log: " + e.Message);
        }
    }

    /// <summary>
    /// Clear the log file
    /// </summary>
    public static void ClearLog() {
        if (instance != null)
        {
            try
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string header = "=== ERROR LOG CLEARED ===" + "\n" + "Date: " + timestamp + "\n\n";
                File.WriteAllText(instance.logFilePath, header);
                Debug.Log("Error log cleared successfully");
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to clear log: " + e.Message);
            }
        }
    }

    /// <summary>
    /// Get the path to the log file
    /// </summary>
    public static string GetLogFilePath() {
        return instance != null ? instance.logFilePath : "";
    }

    /// <summary>
    /// Opens the log file location in the file explorer
    /// </summary>
    public static void OpenLogFileLocation() {
        if (instance != null && !string.IsNullOrEmpty(instance.logFilePath))
        {
            string folder = Path.GetDirectoryName(instance.logFilePath);
            Application.OpenURL("file:///" + folder);
        }
    }
}