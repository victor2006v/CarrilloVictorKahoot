using TMPro;
using UnityEngine;


public class KahootCreatorLayoutManager : MonoBehaviour
{
    [Header("Panels References")]
    [SerializeField] private GameObject numQuestionPanel;
    [SerializeField] private GameObject kahootCreatorPanel;

    [Header("Quizz Instantiate Configuration")]
    [SerializeField] private TMP_InputField numQuestions;
    [SerializeField] private GameObject newQuizzPrefab;
    [SerializeField] private GameObject parentGameObject;
    private int totalQuizz = 0;

    private void Start() {
        if (!numQuestionPanel.activeInHierarchy) numQuestionPanel.SetActive(true);
        if (kahootCreatorPanel == null) {
            Debug.LogWarning("Kahoot Creator Panel is missing");
            return;
        }
        DesactivatePanel(kahootCreatorPanel);
        if (numQuestions == null) { 
            Debug.LogWarning("NumQuestions is missing");
            return;
        }
        if(newQuizzPrefab == null){
            Debug.LogWarning("Quizz Creator is missing");
            return;
        }
        //Force the input to be a number
        numQuestions.contentType = TMP_InputField.ContentType.IntegerNumber;

        numQuestions.onEndEdit.AddListener(CreateKahoot);
    }
    private void OnDisable() {
        numQuestions.onEndEdit.RemoveListener(CreateKahoot);
    }
    private void ActivePanel(GameObject panel){
        panel.SetActive(true);
    }
    private void DesactivatePanel(GameObject panel) { panel.SetActive(false); }

    private void CreateKahoot(string _) {
        DesactivatePanel(numQuestionPanel);
        ActivePanel(kahootCreatorPanel);
        totalQuizz = int.Parse(numQuestions.text);
        totalQuizz--; //Because there's the 0 number
        CreateQuiz();
    }
    private void CreateQuiz() {
        for (int i = 0; i < totalQuizz; i++) {
            Instantiate(newQuizzPrefab, parentGameObject.transform);
        }   
    }
}
