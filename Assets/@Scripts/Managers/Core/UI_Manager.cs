using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager 
{
    int _order = 10;

    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();

    public UI_Scene SceneUI
    {
        get { return _sceneUI; }
        set { _sceneUI = value; }
    }

    UI_Scene _sceneUI = null;

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");

            if (root == null)
                root = new GameObject { name = "@UI_Root" };
            return root;
        }
    }

    public void SetCanvas(GameObject go, bool sort = true, int sortOrder = 0)
    {
        Canvas canvas = Util.GetOrAddComponet<Canvas>(go);
        if (canvas == null)
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.overrideSorting = true;
        }

        CanvasScaler cansvasScaler = go.GetComponent<CanvasScaler>();

        if (cansvasScaler != null)
        {
            cansvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            cansvasScaler.referenceResolution = new Vector2(1080, 1920);
        }
        go.GetOrAddComponent<GraphicRaycaster>();

        // sorting이 들어가야 하는 것인가 아닌가 팝업의 경우에는 내가 띄운 순서대로 쫙 떠야 하니까 sorting이 필요한데 
        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else // SceneUI의 경우 sorting이 필요하지 않기 때문에  이렇게 나눠서 작성
        {
            canvas.sortingOrder = sortOrder;
        }
    }

    public T GetSceneUI<T>() where T : UI_Base
    {
        return _sceneUI as T;
    }

    public T MakeWorldSpaceUI<T>(Transform parent = null , string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"{name}");
        if (parent != null)
            go.transform.SetParent(parent);

        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;

        return Util.GetOrAddComponet<T>(go);
    }


}
