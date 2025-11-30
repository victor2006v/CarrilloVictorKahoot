using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewUser : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputGameObject;
    private Button doneButton;
    private string userName;

    private void Awake() {
        if (inputGameObject == null) {
            Debug.Log("Input GameObject is missing");
            return;
        } 
        doneButton = GetComponent<Button>();
    }
    private void Start() {
        doneButton.onClick.AddListener(ShowMessage);
    }
    private void OnDisable() {
        doneButton.onClick.RemoveListener(ShowMessage);
    }
    private void ShowMessage() {
        userName = inputGameObject.text;
        Debug.Log(userName);
    }
    public string GetUserName() {  return userName; }
}
