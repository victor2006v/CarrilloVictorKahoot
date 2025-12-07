using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;

public class QuizSelectorForLeaderboard : MonoBehaviour {
    [Header("UI References")]
    [SerializeField] private Transform contentContainer;
    [SerializeField] private GameObject quizButtonPrefab;

    private void Start() {
        LoadQuizOptions();
    }

    private void LoadQuizOptions() {
        try
        {
            // Clear existing buttons
            foreach (Transform child in contentContainer)
            {
                Destroy(child.gameObject);
            }

            // Get all quiz files
            string path = Path.Combine(Application.persistentDataPath, "quizzes");

            if (!Directory.Exists(path))
            {
                ErrorLogger.LogWarning("QuizSelectorForLeaderboard", "No quizzes folder found at: " + path);
                Debug.Log("No quizzes folder found");
                return;
            }

            string[] filePaths = Directory.GetFiles(path, "*.json");

            if (filePaths.Length == 0)
            {
                ErrorLogger.LogWarning("QuizSelectorForLeaderboard", "No quiz files found in: " + path);
                Debug.Log("No quizzes found");
                return;
            }

            // Create a button for each quiz
            foreach (string filePath in filePaths)
            {
                try
                {
                    string jsonContent = File.ReadAllText(filePath);
                    FullQuizData quizData = JsonUtility.FromJson<FullQuizData>(jsonContent);

                    if (quizData != null)
                    {
                        CreateQuizButton(quizData.quizName);
                    }
                    else
                    {
                        ErrorLogger.LogError("QuizSelectorForLeaderboard", "Failed to parse quiz from: " + filePath);
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogException("QuizSelectorForLeaderboard.LoadQuizOptions[foreach]", e);
                }
            }

            // Add an "All Quizzes" button at the top
            CreateAllQuizzesButton();
        }
        catch (Exception e)
        {
            ErrorLogger.LogException("QuizSelectorForLeaderboard.LoadQuizOptions", e);
        }
    }

    private void CreateAllQuizzesButton() {
        try
        {
            GameObject button = Instantiate(quizButtonPrefab, contentContainer);
            button.transform.SetAsFirstSibling();

            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = "All Quizzes";
            }

            UnityEngine.UI.Button btn = button.GetComponent<UnityEngine.UI.Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(() => OnQuizSelected(""));
            }
        }
        catch (Exception e)
        {
            ErrorLogger.LogException("QuizSelectorForLeaderboard.CreateAllQuizzesButton", e);
        }
    }

    private void CreateQuizButton(string quizName) {
        try
        {
            GameObject button = Instantiate(quizButtonPrefab, contentContainer);

            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = quizName;
            }

            UnityEngine.UI.Button btn = button.GetComponent<UnityEngine.UI.Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(() => OnQuizSelected(quizName));
            }
        }
        catch (Exception e)
        {
            ErrorLogger.LogException("QuizSelectorForLeaderboard.CreateQuizButton", e);
        }
    }

    private void OnQuizSelected(string quizName) {
        try
        {
            PlayerPrefs.SetString("SelectedLeaderboardQuiz", quizName);
            PlayerPrefs.Save();

            string displayName = string.IsNullOrEmpty(quizName) ? "All Quizzes" : quizName;
            Debug.Log("Selected quiz for leaderboard: " + displayName);

            SceneManager.LoadScene("Leaderboard");
        }
        catch (Exception e)
        {
            ErrorLogger.LogException("QuizSelectorForLeaderboard.OnQuizSelected", e);
        }
    }
}