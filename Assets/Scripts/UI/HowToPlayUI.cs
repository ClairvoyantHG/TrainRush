using UnityEngine;

public class HowToPlayUI : UIBase
{
    private UIButton btnClose;
    private UIButton btnGameStart;

    private void Awake()
    {
        UIButton[] childButtons = GetComponentsInChildren<UIButton>(true);

        for (int i = 0; i < childButtons.Length; i++)
        {
            string objName = childButtons[i].gameObject.name;

            if (objName == "Button_Close")
            {
                btnClose = childButtons[i];
            }
            else if (objName == "Button_Start")
            {
                btnGameStart = childButtons[i];
            }
        }

        if (btnClose == null || btnGameStart == null)
        {
            Debug.LogError("[HowToPlayUI] 하위 버튼의 이름(Button_Close, Button_Start)을 확인해주세요.");
        }
    }

    public override void OnOpen()
    {
        base.OnOpen();

        if (btnClose != null) btnClose.BindOnClickButtonEvent(OnClickClose);
        if (btnGameStart != null) btnGameStart.BindOnClickButtonEvent(OnClickGameStart);
    }

    public override void OnClose()
    {
        base.OnClose();

        if (btnClose != null) btnClose.UnBindOnClickButtonEvent(OnClickClose);
        if (btnGameStart != null) btnGameStart.UnBindOnClickButtonEvent(OnClickGameStart);
    }

    private void OnClickClose()
    {
        UIManager.Instance.CloseUI(UIType.HowToPlayUI);
    }

    private void OnClickGameStart()
    {
        UIManager.Instance.CloseUI(UIType.HowToPlayUI);
        UIManager.Instance.CloseUI(UIType.TitleUI);
        UIManager.Instance.OpenUI(UIRootType.VeryFront, UIType.CountdownUI);
    }
}