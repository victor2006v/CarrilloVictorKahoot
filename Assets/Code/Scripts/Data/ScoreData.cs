using System;
using System.Collections.Generic;

[System.Serializable]
public class ScoreData {
    public string userName;
    public string quizName;
    public int score;
    public int correctAnswers;
    public int totalQuestions;
    public string date; // Format: yyyy-MM-dd HH:mm:ss

    //Constructor to save all the scoreData
    public ScoreData(string user, string quiz, int points, int correct, int total) {
        userName = user;
        quizName = quiz;
        score = points;
        correctAnswers = correct;
        totalQuestions = total;
        date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}

[System.Serializable]
public class ScoreList {
    public List<ScoreData> scores = new List<ScoreData>();
}