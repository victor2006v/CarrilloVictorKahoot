using TMPro;
using UnityEngine;


public class KahootCreatorLayoutManager : MonoBehaviour
{
    [SerializeField] private GameObject questionsPanel;
    [SerializeField] private GameObject answersPanel;
    [SerializeField] private TMP_InputField numQuestions;
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
        //numQuestions.onSubmit.AddListener(CreateAnswers);
    }
    private void OnDisable() {
        //numQuestions.onSubmit.RemoveListener(CreateAnswers);
    }
    private void ActivePanel(GameObject panel){
        panel.SetActive(true);
    }
    private void DesactivatePanel(GameObject panel) {  panel.SetActive(false); }

    private void CreateAnswers() {
        DesactivatePanel(questionsPanel);
        answersPanel.SetActive(true);
    }


}
