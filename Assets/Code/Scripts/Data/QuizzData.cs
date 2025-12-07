using System.Collections.Generic;

[System.Serializable]
public class QuestionData
{
    public string questionTitle;
    public string[] answers; // Array of 4 answers
    public int correctIndex;
}

[System.Serializable]
public class FullQuizData
{
    public string quizName; // Optional: Name of the whole quiz
    public List<QuestionData> questions = new List<QuestionData>();
}