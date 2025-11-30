using UnityEngine;
using UnityEngine.UI;

public class AutoButtonSound : MonoBehaviour
{
    private void Start() {
        Button[] buttons = GetComponentsInChildren<Button>(true);

        foreach (Button b in buttons)
        {
            b.onClick.AddListener(() => AudioManager.Instance.PlayElectricButton());
        }
    }
    private void OnDisable() {
        Button[] buttons = GetComponentsInChildren<Button>(true);

        foreach (Button b in buttons)
        {
            b.onClick.RemoveListener(() => AudioManager.Instance.PlayElectricButton());
        }
    }
}
