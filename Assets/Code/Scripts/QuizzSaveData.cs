using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class QuizzSaveData : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TMP_InputField titleInput;
    [SerializeField] private TMP_InputField[] answerInputs;
    [SerializeField] private TMP_Dropdown correctIndexDropdown;
    public QuestionData GetData()
    {
        QuestionData data = new QuestionData();

        // 1. Get Title
        data.questionTitle = titleInput.text;

        // 2. Get Answers
        data.answers = new string[answerInputs.Length];
        for (int i = 0; i < answerInputs.Length; i++)
        {
            data.answers[i] = answerInputs[i].text;
        }

        // 3. Get Correct Index (From dropdown value 0-3)
        data.correctIndex = correctIndexDropdown.value;

        return data;
    }
}
