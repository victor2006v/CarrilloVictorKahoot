using UnityEngine;
using System.IO;
using System;

public class QuizListLoader : MonoBehaviour {
    [Header("UI References")]
    [SerializeField] private Transform contentContainer;
    [SerializeField] private GameObject quizItemPrefab;

    private void Start() {
        LoadQuizFiles();
    }

    public void LoadQuizFiles() {
        try
        {
            // 1. Clear existing items
            foreach (Transform child in contentContainer)
            {
                Destroy(child.gameObject);
            }

            // 2. Get the path to where we saved the files
            string path = Path.Combine(Application.persistentDataPath, "quizzes");

            // 3. Check if directory exists
            if (!Directory.Exists(path))
            {
                string errorMsg = "Quizzes folder does not exist at: " + path;
                ErrorLogger.LogError("QuizListLoader", errorMsg);
                Debug.Log("Creating quizzes directory...");
                Directory.CreateDirectory(path);
                return;
            }

            // 4. Find all files ending in .json
            string[] filePaths = Directory.GetFiles(path, "*.json");

            if (filePaths.Length == 0)
            {
                string warningMsg = "No quiz files found in: " + path;
                ErrorLogger.LogWarning("QuizListLoader", warningMsg);
                Debug.Log(warningMsg);
                return;
            }

            // 5. Loop through every file found
            foreach (string filePath in filePaths)
            {
                try
                {
                    // Read the text inside the file
                    string jsonContent = File.ReadAllText(filePath);

                    // Convert JSON back to data
                    FullQuizData quizData = JsonUtility.FromJson<FullQuizData>(jsonContent);

                    if (quizData != null)
                    {
                        CreateQuizButton(quizData.quizName, filePath);
                    }
                    else
                    {
                        ErrorLogger.LogError("QuizListLoader", "Failed to parse quiz data from: " + filePath);
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogException("QuizListLoader.LoadQuizFiles[foreach]", e);
                }
            }
        }
        catch (Exception e)
        {
            ErrorLogger.LogException("QuizListLoader.LoadQuizFiles", e);
        }
    }

    private void CreateQuizButton(string name, string path) {
        try
        {
            // Instantiate the button inside the ScrollView
            GameObject newButton = Instantiate(quizItemPrefab, contentContainer);

            // Get the script and set the text
            QuizItemUI uiScript = newButton.GetComponent<QuizItemUI>();
            if (uiScript != null)
            {
                uiScript.Initialize(name, path);
            }
            else
            {
                ErrorLogger.LogError("QuizListLoader", "QuizItemUI component not found on prefab");
            }
        }
        catch (Exception e)
        {
            ErrorLogger.LogException("QuizListLoader.CreateQuizButton", e);
        }
    }
}