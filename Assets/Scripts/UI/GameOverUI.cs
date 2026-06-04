using UnityEngine;

public class GameOverUI : UIBase
{
    private UIButton btnRestart;
    private UIButton btnTitle;

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

        if (btnRestart == null || btnTitle == null)
        {
            Debug.LogError("[GameOverUI] 하위 버튼의 이름(Button_Restart, Button_Title)을 확인해주세요.");
        }
    }

    public override void OnOpen()
    {
        base.OnOpen();

        if (btnRestart != null) btnRestart.BindOnClickButtonEvent(OnClickRestart);
        if (btnTitle != null) btnTitle.BindOnClickButtonEvent(OnClickTitle);
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