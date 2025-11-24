using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonManager : MonoBehaviour
{
    public enum BUTTONS {
        PLAY,
        OPTIONS,
        CONTINUE,
        MAIN_MENU,
        EXIT
    }
    public BUTTONS currentButton;

    private Button button;

    private void Awake() {
        button = GetComponent<Button>();
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
            case BUTTONS.EXIT:
                Application.Quit();
                Debug.Log("Exit");
                break;
        }
    }
}
