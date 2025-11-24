using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class SceneChangerButton : SceneChanger {
    private Button button;

    private void Reset() {
#if UNITY_EDITOR
        Undo.RegisterCompleteObjectUndo(this, "Reset");
        if (!button) button = GetComponent<Button>();
#endif
    }

    private void OnEnable() {
        if (!button) button = GetComponent<Button>();
        button.onClick.AddListener(ChangeScene);
    }

    private void OnDisable() {
        if (!button) button = GetComponent<Button>();
        if (button) button.onClick.RemoveListener(ChangeScene);
    }
}
