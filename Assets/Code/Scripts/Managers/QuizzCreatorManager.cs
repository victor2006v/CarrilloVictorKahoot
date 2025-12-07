using UnityEngine;
using System.IO; // Required for saving files
using TMPro;     // Required for the Name Input Field

public class QuizzCreatorManager : MonoBehaviour
{
    [Header("Settings")]
    // Drag the "Content" object (parent of all questions) here
    [SerializeField] private Transform contentContainer;

    // Drag the input field where the user types the file name
    [SerializeField] private TMP_InputField quizNameInput;

    public void SaveToJSON()
    {
        //Create the object where it will be saved all the data
        FullQuizData fullQuiz = new FullQuizData();

        string finalName = "MyQuiz"; // Default name

        // Set the name (use "MyQuiz" by default if input is not empty or null)
        if (quizNameInput != null && !string.IsNullOrEmpty(quizNameInput.text))
        {
            finalName = quizNameInput.text.Trim();
        }
        fullQuiz.quizName = finalName;

        string quizzesFolderPath = Path.Combine(Application.persistentDataPath, "Quizzes");

        //Creates the folder if not exists
        if (!Directory.Exists(quizzesFolderPath))
        {
            Directory.CreateDirectory(quizzesFolderPath);
            Debug.Log("Quizz folder created");
        }
        //Check for duplicates
        string filePath = Path.Combine(quizzesFolderPath, finalName + ".json");

        // Check if the file exists
        if (File.Exists(filePath))
        {
            Debug.LogError($"Cannot Save: A quiz named '{finalName}' already exists at {filePath}");
            return; // STOP execution here
        }
        
        foreach (Transform child in contentContainer)
        {
            QuizzSaveData questionScript = child.GetComponent<QuizzSaveData>();

            if (questionScript != null)
            {
                fullQuiz.questions.Add(questionScript.GetData());
            }
        }

        // --- Convert and Save ---
        string json = JsonUtility.ToJson(fullQuiz, true);

        File.WriteAllText(filePath, json);

        //With the $ symbol the text is green
        Debug.Log($"Success! Quiz saved to: {filePath}");
    }
}