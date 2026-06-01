using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonBase<UIManager>
{
    [Header("Canvas Roots")]
    [SerializeField] private Transform canvasBgRoot;
    [SerializeField] private Transform canvasMainRoot;
    [SerializeField] private Transform canvasContentRoot;
    [SerializeField] private Transform canvasPopupRoot;
    [SerializeField] private Transform canvasVeryFrontRoot;

    private Dictionary<UIType, UIBase> createdUIDic = new Dictionary<UIType, UIBase>();

    private HashSet<UIType> openedUISet = new HashSet<UIType>();

    public UIBase OpenUI(UIRootType rootType, UIType uiType, bool isInitialHide = false)
    {
        UIBase openedUI = GetCreatedUI(rootType, uiType);

        if (openedUI != null && !openedUISet.Contains(uiType))
        {
            bool isSetActiveOnOpen = !isInitialHide;
            openedUI.gameObject.SetActive(isSetActiveOnOpen);
            openedUISet.Add(uiType);

            openedUI.OnOpen();
        }

        return openedUI;
    }

    public void CloseUI(UIType uiType)
    {
        if (openedUISet.Contains(uiType))
        {
            UIBase openedUi = createdUIDic[uiType];
            openedUi.gameObject.SetActive(false);
            openedUISet.Remove(uiType);

            openedUi.OnClose();
        }
    }

    public T GetUI<T>(UIType uiType) where T : UIBase
    {
        if (createdUIDic.ContainsKey(uiType))
        {
            return createdUIDic[uiType] as T;
        }
        return null;
    }

    private Transform GetRootTransform(UIRootType rootType)
    {
        switch (rootType)
        {
            case UIRootType.Background: return canvasBgRoot;
            case UIRootType.Main: return canvasMainRoot;
            case UIRootType.Content: return canvasContentRoot;
            case UIRootType.Popup: return canvasPopupRoot;
            case UIRootType.VeryFront: return canvasVeryFrontRoot;
            default: return canvasMainRoot;
        }
    }

    private UIBase GetCreatedUI(UIRootType rootType, UIType uiType)
    {
        if (!createdUIDic.ContainsKey(uiType))
        {
            CreateUI(rootType, uiType);
        }
        return createdUIDic[uiType];
    }

    private void CreateUI(UIRootType rootType, UIType uiType)
    {
        string path = "Prefabs/UI/" + uiType.ToString();
        GameObject loadedObj = Resources.Load<GameObject>(path);

        if (loadedObj != null)
        {
            Transform root = GetRootTransform(rootType);
            GameObject gObj = Instantiate(loadedObj, root);
            UIBase uiBase = gObj.GetComponent<UIBase>();

            if (uiBase != null)
            {
                createdUIDic.Add(uiType, uiBase);
                gObj.SetActive(false); 
            }
            else
            {
                Debug.LogError("[UIManager] UIBase 컴포넌트가 없습니다: " + uiType);
            }
        }
        else
        {
            Debug.LogError("[UIManager] UI 프리팹을 찾을 수 없습니다 경로: " + path);
        }
    }
}