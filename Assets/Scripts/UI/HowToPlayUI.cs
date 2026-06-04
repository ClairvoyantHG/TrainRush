using UnityEngine;

public class HowToPlayUI : UIBase
{
    [SerializeField] private UIButton btnClose;

    public override void OnOpen()
    {
        base.OnOpen();
        if (btnClose != null) btnClose.BindOnClickButtonEvent(OnClickClose);
    }

    public override void OnClose()
    {
        base.OnClose();
        if (btnClose != null) btnClose.UnBindOnClickButtonEvent(OnClickClose);
    }

    private void OnClickClose()
    {
        UIManager.Instance.CloseUI(UIType.HowToPlayUI);
    }
}