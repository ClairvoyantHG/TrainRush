using UnityEngine;
using TMPro;

public class GameOverUI : UIBase
{
    private UIButton btnRestart;
    private UIButton btnTitle;
    private TextMeshProUGUI textFinalScore;

    private void Awake()
    {
        UIButton[] childButtons = GetComponentsInChildren<UIButton>(true);

        for (int i = 0; i < childButtons.Length; i++)
        {
            string objName = childButtons[i].gameObject.name;

            if (objName == "Button_Restart")
            {
                btnRestart = childButtons[i];
            }
            else if (objName == "Button_Title")
            {
                btnTitle = childButtons[i];
            }
        }

        TextMeshProUGUI[] childTexts = GetComponentsInChildren<TextMeshProUGUI>(true);
        for (int i = 0; i < childTexts.Length; i++)
        {
            if (childTexts[i].gameObject.name == "Text_FinalScore")
            {
                textFinalScore = childTexts[i];
                break;
            }
        }

        if (btnRestart == null || btnTitle == null || textFinalScore == null)
        {
            Debug.LogError("[GameOverUI] (Button_Restart, Button_Title, Text_FinalScore)을 확인해주세요.");
        }
    }

    public override void OnOpen()
    {
        base.OnOpen();

        if (btnRestart != null) btnRestart.BindOnClickButtonEvent(OnClickRestart);

        if (btnTitle != null) btnTitle.BindOnClickButtonEvent(OnClickTitle);

        if (textFinalScore != null && GameManager.Instance != null)
        {
            int finalScore = GameManager.Instance.CurrentScore;
            textFinalScore.text = "FINAL SCORE\n" + finalScore.ToString("D5");
        }
    }

    public override void OnClose()
    {
        base.OnClose();

        if (btnRestart != null) btnRestart.UnBindOnClickButtonEvent(OnClickRestart);

        if (btnTitle != null) btnTitle.UnBindOnClickButtonEvent(OnClickTitle);
    }

    private void OnClickRestart()
    {
        UIManager.Instance.CloseUI(UIType.GameOverUI);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.RestartGame();
        }
    }

    private void OnClickTitle()
    {
        UIManager.Instance.CloseUI(UIType.GameOverUI);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.GoToTitle();
        }
    }
}