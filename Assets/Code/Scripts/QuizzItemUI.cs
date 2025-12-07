using UnityEngine;
using TMPro;

public class QuizItemUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI quizNameText;

    private string fullFilePath; //Gets the path where the quizz is located
    //The Loader from QuizListLoader configurates the button accesing with this method
    public void Initialize(string name, string path) {
        quizNameText.text = name;
        fullFilePath = path;
    }

    // OnClickOpenquiz in the OnClick button
    public void OnClickOpenQuiz() {
        Debug.Log("User selected the quizz in: " + fullFilePath);

        // Guarda la ruta del quiz seleccionado
        PlayerPrefs.SetString("CurrentQuizPath", fullFilePath);
        //Save the user
        if (!PlayerPrefs.HasKey("CurrentUser"))
        {
            PlayerPrefs.SetString("CurrentUser", "Guest");
        }
        PlayerPrefs.Save();
    }
}