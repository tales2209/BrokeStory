using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITools : MonoSingleton<UITools>
{
    Dictionary<eUIType, BaseObject> DicUI = new Dictionary<eUIType, BaseObject>();

    // DontDestroy 전용
    GameObject SubUIRoot = null;
    Dictionary<eUIType, BaseObject> DicSubUI = new Dictionary<eUIType, BaseObject>();

    Camera _UICamera = null;
    Camera UICamera
    {
        get
        {
            if(_UICamera == null)
            {
                _UICamera = NGUITools.FindCameraForLayer(LayerMask.NameToLayer("UI"));
            }
            return _UICamera;
        }
    }
    
    BaseObject GetUI(eUIType uIType, bool isDontDestroy = false)
    {
        if(isDontDestroy == false)
        {
            if (DicUI.ContainsKey(uIType) == true)
                return DicUI[uIType];
        }
        else
        {
            if (DicSubUI.ContainsKey(uIType) == true)
                return DicSubUI[uIType];
        }

        GameObject makeUI = null;
        BaseObject baseObject = null;
        GameObject prefabUI = Resources.Load("UI/" + uIType.ToString()) as GameObject;

        if(prefabUI != null)
        {
            // isDontDestroy 불값을 통해 삭제 될것과 안될것을 분리시켜 준다.
            if(isDontDestroy == false)
            {
                makeUI = NGUITools.AddChild(UICamera.gameObject, prefabUI);

                baseObject = makeUI.GetComponent<BaseObject>();
                if (baseObject == null)
                {
                    Debug.Log(uIType.ToString() + " 오브젝트에 " + "BaseObject가 연결되어 있지 않습니다.");
                    baseObject = makeUI.AddComponent<BaseObject>();
                }

                DicUI.Add(uIType, baseObject);
            }
            else
            {
                if(SubUIRoot == null)
                {
                    SubRootCreate();
                }

                makeUI = NGUITools.AddChild(SubUIRoot, prefabUI);

                baseObject = makeUI.GetComponent<BaseObject>();
                if (baseObject == null)
                {
                    Debug.Log(uIType.ToString() + " 오브젝트에 " + "BaseObject가 연결되어 있지 않습니다.");
                    baseObject = makeUI.AddComponent<BaseObject>();
                }

                // 로딩바가 신전환이 되도 사라지지 않게 만들어준다.
                DicSubUI.Add(uIType, baseObject);
            }
        }
            return baseObject;
    }

    // baseobject 형으로 반환 필요
    public BaseObject ShowUI(eUIType uIType, bool isSub = false)
    {
        // 꺼져 있는놈을 켜주겠다.
        BaseObject uiObject = GetUI(uIType, isSub);
        if (uiObject != null && uiObject.SelfObject.activeSelf == false)
        {
            uiObject.SelfObject.SetActive(true);
        }

        return uiObject;
    }

    public void HideUI(eUIType uIType, bool isSub = false)
    {
        // 켜져 있는놈만 끄겠다.
        BaseObject uiObject = GetUI(uIType, isSub);
        if (uiObject != null && uiObject.SelfObject.activeSelf == true)
        {
            uiObject.SelfObject.SetActive(false);
        }
    }

    // 로딩바 관련
    public void ShowLoadingUI(float value)
    {
        BaseObject loadingUI = GetUI(eUIType.LoadingUI, true);

        if (loadingUI == null)
            return;

        if (loadingUI.gameObject.activeSelf == false)
            loadingUI.gameObject.SetActive(true);

        loadingUI.ThrowEvent("LoadingValue", value);
    }

    public void Clear()
    {
        foreach (KeyValuePair<eUIType, BaseObject> pair in DicUI)
        {
            Destroy(pair.Value.gameObject);
        }

        DicUI.Clear();
    }

    public void SubRootCreate()
    {
        // SubUIRoot 가 없다면 생성
        if (SubUIRoot == null)
        {
            GameObject subRoot = new GameObject();
            subRoot.transform.SetParent(this.transform);
            SubUIRoot = subRoot;

            SubUIRoot.layer = LayerMask.NameToLayer("UI");
        }

        // SubUIRoot 최신화
        UIRoot uIRoot = UICamera.GetComponentInParent<UIRoot>();

        SubUIRoot.transform.position = uIRoot.transform.position;
        SubUIRoot.transform.localScale = uIRoot.transform.localScale;
    }
}
