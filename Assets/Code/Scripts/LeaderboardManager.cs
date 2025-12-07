using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour {
    [Header("UI References")]
    [SerializeField] private Transform leaderboardContainer;
    [SerializeField] private GameObject leaderboardEntryPrefab;
    [SerializeField] private TextMeshProUGUI playerResultText;

    [Header("Settings")]
    [SerializeField] private int maxEntriesToShow = 10;

    private void Start() {
        ShowPlayerResult();

        // Check if a specific quiz was selected
        string selectedQuiz = PlayerPrefs.GetString("SelectedLeaderboardQuiz", "");

        if (string.IsNullOrEmpty(selectedQuiz))
        {
            LoadLeaderboard(); // Load all quizzes
        }
        else
        {
            LoadLeaderboardForQuiz(selectedQuiz); // Load specific quiz
        }

        // Clear the selection for next time
        PlayerPrefs.DeleteKey("SelectedLeaderboardQuiz");
        PlayerPrefs.Save();
    }

    private void ShowPlayerResult() {
        try
        {
            if (playerResultText == null) return;

            string currentUser = PlayerPrefs.GetString("CurrentUser", "Guest");
            int lastScore = PlayerPrefs.GetInt("LastScore", 0);
            int correct = PlayerPrefs.GetInt("LastCorrectAnswers", 0);
            int total = PlayerPrefs.GetInt("LastTotalQuestions", 0);

            playerResultText.text = currentUser + "\nScore: " + lastScore + "\nCorrect: " + correct + "/" + total;
        }
        catch (Exception e)
        {
            ErrorLogger.LogException("LeaderboardManager.ShowPlayerResult", e);
        }
    }

    public void LoadLeaderboard() {
        try
        {
            // Clear existing entries
            foreach (Transform child in leaderboardContainer)
            {
                Destroy(child.gameObject);
            }

            string scoresPath = Path.Combine(Application.persistentDataPath, "scores.json");

            if (!File.Exists(scoresPath))
            {
                ErrorLogger.LogWarning("LeaderboardManager", "Scores file not found at: " + scoresPath);
                Debug.Log("No scores found yet");
                CreateEntry("No scores yet!", 0);
                return;
            }

            // Load scores
            string json = File.ReadAllText(scoresPath);

            if (string.IsNullOrEmpty(json))
            {
                ErrorLogger.LogWarning("LeaderboardManager", "Scores file is empty");
                CreateEntry("No scores yet!", 0);
                return;
            }

            ScoreList scoreList = JsonUtility.FromJson<ScoreList>(json);

            if (scoreList == null || scoreList.scores == null || scoreList.scores.Count == 0)
            {
                ErrorLogger.LogWarning("LeaderboardManager", "No valid scores found in file");
                CreateEntry("No scores yet!", 0);
                return;
            }

            // Sort by score (highest first)
            List<ScoreData> sortedScores = scoreList.scores
                .OrderByDescending(s => s.score)
                .Take(maxEntriesToShow)
                .ToList();

            // Create entries
            int rank = 1;
            foreach (ScoreData score in sortedScores)
            {
                string displayName = "#" + rank + " - " + score.userName + " (" + score.quizName + ")";
                CreateEntry(displayName, score.score);
                rank++;
            }
        }
        catch (Exception e)
        {
            ErrorLogger.LogException("LeaderboardManager.LoadLeaderboard", e);
            CreateEntry("Error loading leaderboard", 0);
        }
    }

    private void CreateEntry(string playerInfo, int score) {
        try
        {
            GameObject entry = Instantiate(leaderboardEntryPrefab, leaderboardContainer);

            // Get the LeaderboardEntry component
            LeaderboardEntry entryScript = entry.GetComponent<LeaderboardEntry>();

            if (entryScript != null)
            {
                entryScript.SetData(playerInfo, score);
            }
            else
            {
                // Fallback: try to get TextMeshProUGUI components directly
                TextMeshProUGUI[] texts = entry.GetComponentsInChildren<TextMeshProUGUI>();

                if (texts.Length >= 2)
                {
                    texts[0].text = playerInfo;
                    texts[1].text = score.ToString();
                }
                else if (texts.Length == 1)
                {
                    texts[0].text = playerInfo + " - " + score + " pts";
                }
                else
                {
                    ErrorLogger.LogWarning("LeaderboardManager", "Could not set entry data - no valid components found");
                }
            }
        }
        catch (Exception e)
        {
            ErrorLogger.LogException("LeaderboardManager.CreateEntry", e);
        }
    }

    // Optional: Filter by specific quiz
    public void LoadLeaderboardForQuiz(string quizName) {
        try
        {
            foreach (Transform child in leaderboardContainer)
            {
                Destroy(child.gameObject);
            }

            string scoresPath = Path.Combine(Application.persistentDataPath, "scores.json");

            if (!File.Exists(scoresPath))
            {
                ErrorLogger.LogWarning("LeaderboardManager", "Scores file not found at: " + scoresPath);
                CreateEntry("No scores for this quiz", 0);
                return;
            }

            string json = File.ReadAllText(scoresPath);
            ScoreList scoreList = JsonUtility.FromJson<ScoreList>(json);

            if (scoreList == null || scoreList.scores == null)
            {
                ErrorLogger.LogWarning("LeaderboardManager", "Failed to parse scores data");
                CreateEntry("No scores for this quiz", 0);
                return;
            }

            List<ScoreData> filteredScores = scoreList.scores
                .Where(s => s.quizName == quizName)
                .OrderByDescending(s => s.score)
                .Take(maxEntriesToShow)
                .ToList();

            if (filteredScores.Count == 0)
            {
                ErrorLogger.LogWarning("LeaderboardManager", "No scores found for quiz: " + quizName);
                CreateEntry("No scores for this quiz", 0);
                return;
            }

            int rank = 1;
            foreach (ScoreData score in filteredScores)
            {
                string displayName = "#" + rank + " - " + score.userName;
                CreateEntry(displayName, score.score);
                rank++;
            }
        }
        catch (Exception e)
        {
            ErrorLogger.LogException("LeaderboardManager.LoadLeaderboardForQuiz", e);
            CreateEntry("Error loading leaderboard", 0);
        }
    }
}