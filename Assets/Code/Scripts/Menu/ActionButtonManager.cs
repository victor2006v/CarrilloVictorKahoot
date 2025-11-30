using UnityEngine;
using UnityEngine.UI;

public class ActionButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject newUserPanel;
    [SerializeField] private GameObject selectionUserPanel;
    public enum BUTTONS {
        PLAY,
        OPTIONS,
        CONTINUE,
        MAIN_MENU,
        NEW_USER,
        EXIT
    }
    public BUTTONS currentButton;

    private Button button;

    private void Awake() {
        button = GetComponent<Button>();
    }
    private void Start() {
        if (newUserPanel == null) {
            Debug.LogWarning("New User Panel is missing");
            return;
        }
    }
    private void OnEnable() {
        if (button != null) button.onClick.AddListener(OnButtonPressed);
    }

    private void OnDisable() {
        button.onClick.RemoveListener(OnButtonPressed);
    }

    private void OnButtonPressed(){
        switch (currentButton)
        {
            case BUTTONS.PLAY:
                break;
            case BUTTONS.OPTIONS:
                break;
            case BUTTONS.CONTINUE:
                break;
            case BUTTONS.MAIN_MENU:
                break;
            case BUTTONS.NEW_USER:
                selectionUserPanel.SetActive(false);
                newUserPanel.SetActive(true);
                break;
            case BUTTONS.EXIT:
                Application.Quit();
                Debug.Log("Exit");
                break;
        }
    }
}
