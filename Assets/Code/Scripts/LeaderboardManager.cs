using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour {
    [Header("UI References")]
    [SerializeField] private Transform leaderboardContainer; // Content of ScrollView
    [SerializeField] private GameObject leaderboardEntryPrefab; // Prefab for each entry
    [SerializeField] private TextMeshProUGUI playerResultText; // Shows current player result

    [Header("Settings")]
    [SerializeField] private int maxEntriesToShow = 10; // Top 10

    private void Start() {
        // 1. Mostrar la puntuación reciente del jugador
        ShowPlayerResult();

        // 2. Recuperar qué quiz seleccionó el usuario en la escena anterior
        string selectedQuiz = PlayerPrefs.GetString("SelectedLeaderboardQuiz", "");

        // 3. Decidir qué lista cargar
        if (string.IsNullOrEmpty(selectedQuiz) || selectedQuiz == "All Quizzes")
        {
            LoadLeaderboard();
        }
        else
        {
            LoadLeaderboardForQuiz(selectedQuiz);
        }
    }

    private void ShowPlayerResult() {
        if (playerResultText == null)
        {
            Debug.LogWarning("Falta asignar 'Player Result Text' en el Inspector del LeaderboardManager");
            return;
        }

        string currentUser = PlayerPrefs.GetString("CurrentUser", "Guest");
        int lastScore = PlayerPrefs.GetInt("LastScore", 0);
        int correct = PlayerPrefs.GetInt("LastCorrectAnswers", 0);
        int total = PlayerPrefs.GetInt("LastTotalQuestions", 0);

        playerResultText.text = $"Jugador: {currentUser}\nPuntuación: {lastScore}\nAciertos: {correct}/{total}";
    }

    public void LoadLeaderboard() {
        ClearContainer();

        string scoresPath = Path.Combine(Application.persistentDataPath, "scores.json");
        if (!File.Exists(scoresPath))
        {
            CreateEntry("No hay puntuaciones", "-", 0, 0, 0);
            return;
        }

        string json = File.ReadAllText(scoresPath);
        ScoreList scoreList = JsonUtility.FromJson<ScoreList>(json);

        if (scoreList == null || scoreList.scores.Count == 0)
        {
            CreateEntry("No hay puntuaciones", "-", 0, 0, 0);
            return;
        }

        ProcessAndShowScores(scoreList.scores);
    }

    public void LoadLeaderboardForQuiz(string quizName) {
        ClearContainer();

        string scoresPath = Path.Combine(Application.persistentDataPath, "scores.json");
        if (!File.Exists(scoresPath))
        {
            CreateEntry("No hay puntuaciones", "-", 0, 0, 0);
            return;
        }

        string json = File.ReadAllText(scoresPath);
        ScoreList scoreList = JsonUtility.FromJson<ScoreList>(json);

        // Filtrar por nombre del quiz
        List<ScoreData> filteredScores = scoreList.scores
            .Where(s => s.quizName == quizName)
            .ToList();

        if (filteredScores.Count == 0)
        {
            CreateEntry("Sin puntuaciones para este quiz", "-", 0, 0, 0);
            return;
        }

        ProcessAndShowScores(filteredScores);
    }

    private void ProcessAndShowScores(List<ScoreData> scoresToList) {
        // Ordenar y limitar
        var sortedScores = scoresToList
            .OrderByDescending(s => s.score)
            .Take(maxEntriesToShow)
            .ToList();

        int rank = 1;
        foreach (ScoreData score in sortedScores)
        {
            // Usamos una pequeña comprobación visual para destacar al usuario actual si coincide
            string displayName = $"#{rank} - {score.userName}";
            CreateEntry(displayName, score.quizName, score.score, score.correctAnswers, score.totalQuestions);
            rank++;
        }
    }

    private void ClearContainer() {
        foreach (Transform child in leaderboardContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void CreateEntry(string playerName, string quizName, int score, int correct, int total) {
        GameObject entry = Instantiate(leaderboardEntryPrefab, leaderboardContainer);

        // Intenta usar el script LeaderboardEntry si existe (es más limpio)
        LeaderboardEntry entryScript = entry.GetComponent<LeaderboardEntry>();
        if (entryScript != null)
        {
            entryScript.SetData(playerName, score);
            // Si quieres pasar más datos tendrás que modificar LeaderboardEntry.cs
            return;
        }

        // Si no hay script, usa GetComponentsInChildren como antes
        TextMeshProUGUI[] texts = entry.GetComponentsInChildren<TextMeshProUGUI>();
        if (texts.Length >= 3)
        {
            texts[0].text = playerName;
            texts[1].text = quizName;
            texts[2].text = $"{score} pts";
        }
        else if (texts.Length > 0)
        {
            texts[0].text = $"{playerName} | {score} pts";
        }
    }
}