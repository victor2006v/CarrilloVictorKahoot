using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour{
    private TextMeshProUGUI timerText;
    [Header("Time variables")]
    private float currentTime;
    private int timeDisplay;

    private void Awake()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        currentTime = 15f;
    }

    private void Update()
    {
        if (currentTime <= 0f) return;

        currentTime = CountDown(currentTime);
        ChangeColor();
    }

    private float CountDown(float time){
        time -= Time.deltaTime;

        timeDisplay = Mathf.FloorToInt(time);
        if (timeDisplay <= 5) timerText.color = Color.red;
        if (timeDisplay <= 0){
            timeDisplay = 0;
            return 0f;
        }

        return time;
    }
    private void ChangeColor(){
        timerText.text = timeDisplay.ToString();
        if (timeDisplay <= 5)
            timerText.color = Color.red;
    }
}
