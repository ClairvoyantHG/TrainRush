using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro; 

public class UIButton : MonoBehaviour
{
    [SerializeField] private Button buttonBase;
    [SerializeField] private TextMeshProUGUI buttonText; 
    [SerializeField] private Image imageBase;
    [SerializeField] private Image imageSelect;

    private void Awake()
    {
        InitUIButton();
        SetDefaultUI();
    }

    private void OnEnable()
    {
        BindOnClickButtonEvent(OnClickSetSelectUI);
    }

    private void OnDisable()
    {
        if (buttonBase != null)
        {
            buttonBase.onClick.RemoveAllListeners();
        }
    }

    private void SetDefaultUI()
    {
        if (imageSelect != null)
        {
            imageSelect.gameObject.SetActive(false);
        }
    }

    private void InitUIButton()
    {
        if (buttonBase == null)
        {
            Button button = GetComponentInChildren<Button>();
            if (button != null)
            {
                buttonBase = button;
            }
        }

        if (buttonText == null)
        {
            TextMeshProUGUI textComp = GetComponentInChildren<TextMeshProUGUI>();
            if (textComp != null)
            {
                buttonText = textComp;
            }
        }
    }

    public void BindOnClickButtonEvent(Action onClickCallback)
    {
        if (buttonBase == null || onClickCallback == null) return;
        buttonBase.onClick.AddListener(new UnityAction(onClickCallback));
    }

    public void UnBindOnClickButtonEvent(Action onClickCallback)
    {
        if (buttonBase == null || onClickCallback == null) return;
        buttonBase.onClick.RemoveListener(new UnityAction(onClickCallback));
    }

    public void ChangeButtonText(string buttonStr)
    {
        if (buttonText == null) return;
        buttonText.text = buttonStr; 
    }

    private void OnClickSetSelectUI()
    {
        if (imageSelect != null)
        {
            bool currentActive = imageSelect.gameObject.activeSelf;
            imageSelect.gameObject.SetActive(!currentActive);
        }
    }
}