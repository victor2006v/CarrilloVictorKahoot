using UnityEngine;
using UnityEngine.UI;
using static ActionButtonManager;
public class LightningAnimation : MonoBehaviour
{
    [SerializeField] private GameObject lightningAnimation;

    private void Start() {
        Button[] buttons = GetComponentsInChildren<Button>(true);

        foreach (Button b in buttons)
        {
            b.onClick.AddListener(() => ActiveAnimation());
        }
    }
    private void OnDisable() {
        Button[] buttons = GetComponentsInChildren<Button>(true);
        foreach (Button b in buttons)
        {
            b.onClick.RemoveListener(() => ActiveAnimation());
        }
    }
    private void ActiveAnimation(){
        lightningAnimation.SetActive(true);
    }
}
