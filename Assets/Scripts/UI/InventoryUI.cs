using UnityEngine;
using TMPro;

public class InventoryUI : UIBase
{
    private UIButton btnClose;
    private UIButton btnGameStart;
    private UIButton btnUseHpPlus;
    private UIButton btnUseSpeedPlus;
    private TextMeshProUGUI textItem01Amount;
    private TextMeshProUGUI textItem02Amount;

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
            else if (objName == "Button_Item_01")
            {
                btnUseHpPlus = childButtons[i];
            }
            else if (objName == "Button_Item_02")
            {
                btnUseSpeedPlus = childButtons[i];
            }
        }

        if (btnClose == null || btnGameStart == null)
        {
            Debug.LogError("[InventoryUI] 하위 버튼의 이름(Button_Close, Button_Start)을 확인해주세요.");
        }

        TextMeshProUGUI[] childTexts = GetComponentsInChildren<TextMeshProUGUI>(true);

        for (int i = 0; i < childTexts.Length; i++)
        {
            string objName = childTexts[i].gameObject.name;

            if (objName == "Text_Item_01_Amount")
            {
                textItem01Amount = childTexts[i];
                textItem01Amount.text = InventoryManager.Instance.CurrentHpMaterial.ToString();
            }
            else if (objName == "Text_Item_02_Amount")
            {
                textItem02Amount = childTexts[i];
                textItem02Amount.text = InventoryManager.Instance.CurrentSpeedMaterial.ToString();
            }
        }

        if (btnClose == null || btnGameStart == null)
        {
            Debug.LogError("[InventoryUI] 하위 버튼의 이름(Text_Item_01_Amount, Text_Item_02_Amount)을 확인해주세요.");
        }
    }

    public override void OnOpen()
    {
        base.OnOpen();

        if (btnClose != null) btnClose.BindOnClickButtonEvent(OnClickClose);
        if (btnGameStart != null) btnGameStart.BindOnClickButtonEvent(OnClickGameStart);
        if (btnUseHpPlus != null) btnUseHpPlus.BindOnClickButtonEvent(OnClickUseHpPlus);
        if (btnUseSpeedPlus != null) btnUseSpeedPlus.BindOnClickButtonEvent(OnClickUseSpeedPlus);
    }

    public override void OnClose()
    {
        base.OnClose();

        if (btnClose != null) btnClose.UnBindOnClickButtonEvent(OnClickClose);
        if (btnGameStart != null) btnGameStart.UnBindOnClickButtonEvent(OnClickGameStart);
        if (btnUseHpPlus != null) btnUseHpPlus.UnBindOnClickButtonEvent(OnClickUseHpPlus);
        if (btnUseSpeedPlus != null) btnUseSpeedPlus.UnBindOnClickButtonEvent(OnClickUseSpeedPlus);
    }

    private void OnClickClose()
    {
        UIManager.Instance.CloseUI(UIType.InventoryUI);
    }

    private void OnClickGameStart()
    {
        UIManager.Instance.CloseUI(UIType.InventoryUI);
        UIManager.Instance.CloseUI(UIType.TitleUI);
        UIManager.Instance.OpenUI(UIRootType.VeryFront, UIType.CountdownUI);
    }

    private void OnClickUseHpPlus()
    {

    }

    private void OnClickUseSpeedPlus()
    {

    }
}