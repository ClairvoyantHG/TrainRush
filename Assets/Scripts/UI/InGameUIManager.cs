using UnityEngine;

public class InGameUIManager : SingletonBase<InGameUIManager>
{
    [SerializeField] private GameObject chaserBorderUI; 
    [SerializeField] private Animator chaserAnimator;   
    protected override void Awake()
    {
        base.Awake();
        if (chaserBorderUI != null)
        {
            chaserBorderUI.SetActive(false); 
        }
    }

    public void ShowChaserWarning()
    {
        if (chaserBorderUI != null)
        {
            chaserBorderUI.SetActive(true);
        }
    }
}