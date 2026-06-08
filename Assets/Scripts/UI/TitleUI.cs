using UnityEngine;

public class TitleUI : UIBase
{
    private UIButton btnGameStart;
    private UIButton btnHowToPlay;
    private UIButton btnQuit;
    private UIButton btnInventory;

    private void Awake()
    {
        UIButton[] childButtons = GetComponentsInChildren<UIButton>(true);

        for (int i = 0; i < childButtons.Length; i++)
        {
            string objName = childButtons[i].gameObject.name;

            if (objName == "Button_Start")
            {
                btnGameStart = childButtons[i];
            }
            else if (objName == "Button_HowToPlay")
            {
                btnHowToPlay = childButtons[i];
            }
            else if (objName == "Button_Quit")
            {
                btnQuit = childButtons[i];
            }
            else if (objName == "Button_Inventory")
            {
                btnInventory = childButtons[i];
            }
        }

        if (btnGameStart == null || btnHowToPlay == null || btnQuit == null || btnInventory == null)
        {
            Debug.LogError("[TitleUI] 하위 버튼 오브젝트의 이름(Button_Start, Button_HowToPlay, Button_Quit)을 확인해주세요.");
        }
    }

    public override void OnOpen()
    {
        base.OnOpen();

        if (btnGameStart != null) btnGameStart.BindOnClickButtonEvent(OnClickGameStart);
        if (btnHowToPlay != null) btnHowToPlay.BindOnClickButtonEvent(OnClickHowToPlay);
        if (btnQuit != null) btnQuit.BindOnClickButtonEvent(OnClickQuit);
        if (btnInventory != null) btnInventory.BindOnClickButtonEvent(OnClickInventory);
    }

    public override void OnClose()
    {
        base.OnClose();

        if (btnGameStart != null) btnGameStart.UnBindOnClickButtonEvent(OnClickGameStart);
        if (btnHowToPlay != null) btnHowToPlay.UnBindOnClickButtonEvent(OnClickHowToPlay);
        if (btnQuit != null) btnQuit.UnBindOnClickButtonEvent(OnClickQuit);
    }

    private void OnClickGameStart()
    {
        UIManager.Instance.CloseUI(UIType.TitleUI);
        UIManager.Instance.OpenUI(UIRootType.VeryFront, UIType.CountdownUI);
    }

    private void OnClickHowToPlay()
    {
        UIManager.Instance.OpenUI(UIRootType.Popup, UIType.HowToPlayUI);
    }

    private void OnClickQuit()
    {
        Debug.Log("게임을 종료합니다.");
        Application.Quit();
    }

    private void OnClickInventory()
    {
        UIManager.Instance.OpenUI(UIRootType.Popup, UIType.InventoryUI);
    }
}