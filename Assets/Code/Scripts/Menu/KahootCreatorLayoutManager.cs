using TMPro;
using UnityEngine;


public class KahootCreatorLayoutManager : MonoBehaviour
{
    [SerializeField] private GameObject questionsPanel;
    [SerializeField] private GameObject answersPanel;
    [SerializeField] private TMP_InputField numQuestions;
    [SerializeField] private GameObject newQuizzPrefab;

    private int totalQuizz = 0;
    private void Start() {
        if (!questionsPanel.activeInHierarchy) questionsPanel.SetActive(true);
        if (answersPanel == null) {
            Debug.LogWarning("AnswersPanel is missing");
            return;
        }
        DesactivatePanel(answersPanel);
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

        numQuestions.onEndEdit.AddListener(CreateAnswers);
    }
    private void OnDisable() {
        numQuestions.onEndEdit.RemoveListener(CreateAnswers);
    }
    private void ActivePanel(GameObject panel){
        panel.SetActive(true);
    }
    private void DesactivatePanel(GameObject panel) {  panel.SetActive(false); }

    private void CreateAnswers(string _) {
        DesactivatePanel(questionsPanel);
        answersPanel.SetActive(true);
        totalQuizz = int.Parse(numQuestions.text);
        CreateQuiz();
    }
    private void CreateQuiz() {
        for (int i = 0; i < totalQuizz; i++) {
            Instantiate(newQuizzPrefab, answersPanel.transform);
        }
        
    }
    


}
