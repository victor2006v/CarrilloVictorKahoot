using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class QuizzGameManager : MonoBehaviour {
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI questionTitleText;
    [SerializeField] private TextMeshProUGUI[] answerButtonTexts;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Settings")]
    [SerializeField] private float timePerQuestion = 15f;
    [SerializeField] private float timeToWaitAfterAnswer = 2f;
    [SerializeField] private int pointsPerCorrectAnswer = 100;
    [SerializeField] private int pointsPerSecondLeft = 10;
    [SerializeField] private string leaderboardSelectorScene = "LeaderboardSelector"; // Nueva escena

    [Header("Colors Configuration")]
    private Color wrongColor = Color.red;
    private Color correctColor = Color.green;
    private Color defaultColor = Color.black;

    // Internal Variables
    private FullQuizData currentQuiz;
    private int currentQuestionIndex = 0;
    private float currentTimer;
    private bool isGameActive = false;

    //Score variables
    private int totalScore = 0;
    private int correctAnswersCount = 0;

    private void Start() {
        LoadData();
    }

    private void Update() {
        if (!isGameActive) return;

        currentTimer -= Time.deltaTime;
        timerText.text = Mathf.CeilToInt(Mathf.Max(0, currentTimer)).ToString();

        if (currentTimer <= 0)
        {
            isGameActive = false;
            StartCoroutine(WaitAndLoadNext());
        }
    }

    private void LoadData() {
        string path = PlayerPrefs.GetString("CurrentQuizPath");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            currentQuiz = JsonUtility.FromJson<FullQuizData>(json);

            currentQuestionIndex = 0;
            totalScore = 0;
            correctAnswersCount = 0;
            UpdateScoreDisplay();
            ShowQuestion();
        }
        else
        {
            Debug.LogError("File not found!");
            questionTitleText.text = "Error: Quiz not found";
        }
    }

    private void ShowQuestion() {
        ResetTextColors();

        if (currentQuestionIndex >= currentQuiz.questions.Count)
        {
            EndGame();
            return;
        }

        QuestionData q = currentQuiz.questions[currentQuestionIndex];
        questionTitleText.text = q.questionTitle;

        for (int i = 0; i < answerButtonTexts.Length; i++)
        {
            if (i < q.answers.Length)
                answerButtonTexts[i].text = q.answers[i];
            else
                answerButtonTexts[i].text = "";
        }

        currentTimer = timePerQuestion;
        timerText.text = timePerQuestion.ToString();
        isGameActive = true;
    }

    private void ResetTextColors() {
        foreach (TextMeshProUGUI text in answerButtonTexts)
        {
            text.color = defaultColor;
        }
    }

    public void OnAnswerClicked(int buttonIndex) {
        if (!isGameActive) return;
        isGameActive = false;

        int correctIndex = currentQuiz.questions[currentQuestionIndex].correctIndex;

        for (int i = 0; i < answerButtonTexts.Length; i++)
        {
            if (i == correctIndex)
                answerButtonTexts[i].color = correctColor;
            else
                answerButtonTexts[i].color = wrongColor;
        }

        if (buttonIndex == correctIndex)
        {
            correctAnswersCount++;
            int timeBonus = Mathf.FloorToInt(currentTimer) * pointsPerSecondLeft;
            int questionPoints = pointsPerCorrectAnswer + timeBonus;
            totalScore += questionPoints;

            Debug.Log($"¡Correct! +{questionPoints} points");
        }
        else
        {
            Debug.Log("Incorrect - 0 points");
        }

        UpdateScoreDisplay();
        StartCoroutine(WaitAndLoadNext());
    }

    private void UpdateScoreDisplay() {
        if (scoreText != null)
        {
            scoreText.text = totalScore.ToString();
        }
    }

    private IEnumerator WaitAndLoadNext() {
        yield return new WaitForSeconds(timeToWaitAfterAnswer);
        LoadNextQuestion();
    }

    private void LoadNextQuestion() {
        currentQuestionIndex++;
        ShowQuestion();
    }

    private void EndGame() {
        timerText.text = "0";
        isGameActive = false;
        questionTitleText.text = "¡Quiz Finished!";
        SaveScore();
        StartCoroutine(GoToLeaderboardSelector());
    }

    private void SaveScore() {
        string currentUser = PlayerPrefs.GetString("CurrentUser", "Guest");

        ScoreData newScore = new ScoreData(
            currentUser,
            currentQuiz.quizName,
            totalScore,
            correctAnswersCount,
            currentQuiz.questions.Count
        );

        string scoresPath = Path.Combine(Application.persistentDataPath, "scores.json");
        ScoreList scoreList = new ScoreList();

        if (File.Exists(scoresPath))
        {
            string json = File.ReadAllText(scoresPath);
            scoreList = JsonUtility.FromJson<ScoreList>(json);
            if (scoreList == null) scoreList = new ScoreList();
        }

        scoreList.scores.Add(newScore);

        string jsonToSave = JsonUtility.ToJson(scoreList, true);
        File.WriteAllText(scoresPath, jsonToSave);

        Debug.Log($"Score saved: {currentUser} - {totalScore} points ({correctAnswersCount}/{currentQuiz.questions.Count} correct)");

        PlayerPrefs.SetInt("LastScore", totalScore);
        PlayerPrefs.SetInt("LastCorrectAnswers", correctAnswersCount);
        PlayerPrefs.SetInt("LastTotalQuestions", currentQuiz.questions.Count);
        PlayerPrefs.Save();
    }

    private IEnumerator GoToLeaderboardSelector() {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(leaderboardSelectorScene);
    }
}