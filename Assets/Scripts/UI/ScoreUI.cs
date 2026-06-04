using UnityEngine;
using TMPro;

public class ScoreUI : UIBase
{
    private TextMeshProUGUI scoreText;

    private int lastDisplayedScore = -1;

    private void Awake()
    {
        scoreText = GetComponentInChildren<TextMeshProUGUI>();

        if (scoreText == null)
        {
            Debug.LogError("[InGameUI] 하위에 TextMeshProUGUI 컴포넌트가 없습니다!");
        }
    }

    public override void OnOpen()
    {
        base.OnOpen();
        lastDisplayedScore = -1;
    }

    private void Update()
    {
        if (GameManager.Instance == null || scoreText == null) return;

        int currentScore = GameManager.Instance.CurrentScore;

        if (currentScore != lastDisplayedScore)
        {
            lastDisplayedScore = currentScore;
            scoreText.text = "SCORE : " + currentScore.ToString("D5");
        }
    }
}