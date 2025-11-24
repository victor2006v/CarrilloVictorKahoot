using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextColorChanger : MonoBehaviour
{
    private TextMeshProUGUI textToChange;

    private void Awake() {
        textToChange = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Update() {
        if (textToChange == null) return;
        //Selected color
        if (EventSystem.current.currentSelectedGameObject == gameObject) textToChange.color = Color.red;
        //Default color
        else textToChange.color = Color.white;
    }
}
