using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;

public class QuizSelectorForLeaderboard : MonoBehaviour {
    [Header("UI References")]
    [SerializeField] private Transform contentContainer; // Content of ScrollView
    [SerializeField] private GameObject quizButtonPrefab; // Button prefab

    private void Start() {
        LoadQuizOptions();
    }

    private void LoadQuizOptions() {
        // Clear existing buttons
        foreach (Transform child in contentContainer)
        {
            Destroy(child.gameObject);
        }

        // Get all quiz files
        string path = Path.Combine(Application.persistentDataPath, "quizzes");

        if (!Directory.Exists(path))
        {
            Debug.Log("No quizzes folder found");
            return;
        }

        string[] filePaths = Directory.GetFiles(path, "*.json");

        if (filePaths.Length == 0)
        {
            Debug.Log("No quizzes found");
            return;
        }

        // Create a button for each quiz
        foreach (string filePath in filePaths)
        {
            string jsonContent = File.ReadAllText(filePath);
            FullQuizData quizData = JsonUtility.FromJson<FullQuizData>(jsonContent);

            if (quizData != null)
            {
                CreateQuizButton(quizData.quizName);
            }
        }

        // Add an "All Quizzes" button at the top
        CreateAllQuizzesButton();
    }

    private void CreateAllQuizzesButton() {
        GameObject button = Instantiate(quizButtonPrefab, contentContainer);
        button.transform.SetAsFirstSibling(); // Put it at the top

        // Get the text component
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            buttonText.text = "All Quizzes";
        }

        // Add click event
        UnityEngine.UI.Button btn = button.GetComponent<UnityEngine.UI.Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(() => OnQuizSelected(""));
        }
    }

    private void CreateQuizButton(string quizName) {
        GameObject button = Instantiate(quizButtonPrefab, contentContainer);

        // Get the text component
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            buttonText.text = quizName;
        }

        // Add click event
        UnityEngine.UI.Button btn = button.GetComponent<UnityEngine.UI.Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(() => OnQuizSelected(quizName));
        }
    }

    private void OnQuizSelected(string quizName) {
        // Save selected quiz name
        PlayerPrefs.SetString("SelectedLeaderboardQuiz", quizName);
        PlayerPrefs.Save();

        Debug.Log($"Selected quiz for leaderboard: {(string.IsNullOrEmpty(quizName) ? "All Quizzes" : quizName)}");

        // Go to leaderboard scene
        SceneManager.LoadScene("Leaderboard");
    }
}