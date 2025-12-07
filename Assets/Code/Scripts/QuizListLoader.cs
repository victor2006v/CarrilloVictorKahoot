using UnityEngine;
using System.IO; // Necessary for file handling

public class QuizListLoader : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform contentContainer; // The Content object of your ScrollView
    [SerializeField] private GameObject quizItemPrefab;  // The Button Prefab with QuizItemUI script

    private void Start()
    {
        LoadQuizFiles();
    }

    public void LoadQuizFiles()
    {
        // 1. Clear existing items (to prevent duplicates if we reload)
        foreach (Transform child in contentContainer)
        {
            Destroy(child.gameObject);
        }

        // 2. Get the path to where we saved the files
        string path = Path.Combine(Application.persistentDataPath, "quizzes");

        // 3. Find all files ending in .json
        string[] filePaths = Directory.GetFiles(path, "*.json");

        if (filePaths.Length == 0)
        {
            Debug.Log("No quizzes found in " + path);
            return;
        }

        // 4. Loop through every file found
        foreach (string filePath in filePaths)
        {
            // Read the text inside the file
            string jsonContent = File.ReadAllText(filePath);

            // Convert JSON back to data so we can read the Title
            FullQuizData quizData = JsonUtility.FromJson<FullQuizData>(jsonContent);

            if (quizData != null)
            {
                CreateQuizButton(quizData.quizName, filePath);
            }
        }
    }

    private void CreateQuizButton(string name, string path)
    {
        // Instantiate the button inside the ScrollView
        GameObject newButton = Instantiate(quizItemPrefab, contentContainer);

        // Get the script and set the text
        QuizItemUI uiScript = newButton.GetComponent<QuizItemUI>();
        if (uiScript != null)
        {
            uiScript.Initialize(name, path);
        }
    }
}