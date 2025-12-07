using UnityEngine;
using TMPro;

public class LeaderboardEntry : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI userNameText;
    [SerializeField] private TextMeshProUGUI scoreText;

    public void SetData(string userName, int score) {
        userNameText.text = userName;
        scoreText.text = score.ToString();
    }
}