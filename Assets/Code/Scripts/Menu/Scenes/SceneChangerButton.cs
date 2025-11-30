using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Button))]
public class SceneChangerButton : SceneChanger {
    private Button button;

    private void OnEnable() {
        button = GetComponent<Button>();
        button.onClick.AddListener(ChangeScene);
    }

    private void OnDisable() {
        if (button != null)
        {
            button.onClick.RemoveListener(ChangeScene);
        }
    }
}
