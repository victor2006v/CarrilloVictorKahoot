using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Required to access the Button component

[RequireComponent(typeof(Button))] // This forces the object to have a Button
public class SceneChanger : MonoBehaviour {
    [Header("Select Scene")]
    public SceneField targetScene;

    private Button _menuButton;

    private void Awake() {
        // 1. Get the button on this same object
        _menuButton = GetComponent<Button>();

        // 2. Add the listener automatically
        if (_menuButton != null)
        {
            _menuButton.onClick.AddListener(ChangeScene);
        }
    }

    private void OnDestroy() {
        // Good practice: Clean up listeners when the object is destroyed
        if (_menuButton != null)
        {
            _menuButton.onClick.RemoveListener(ChangeScene);
        }
    }

    // This function is now called automatically by the listener
    private void ChangeScene() {
        // Check if the scene reference is empty
        if (targetScene == null || string.IsNullOrEmpty(targetScene))
        {
            Debug.LogError("Button '{gameObject.name}' has no Scene selected!", gameObject);
            return;
        }
        SceneManager.LoadScene(targetScene);
    }
}