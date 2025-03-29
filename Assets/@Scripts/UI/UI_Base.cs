using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI들의 최상위 조상 
/// InitBase를 넣어주면서 초기화 루틴을 만들어 주고 
/// </summary>
public class UI_Base : InitBase
{
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    private void Awake()
    {
        Init();// 초기화 관련 루틴 
    }

    /// <summary>
    /// 소스 코드 안에서 해당 아이들을 찾아오는 방법 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] name = Enum.GetNames(type); // 해당 타입으로 이름을 찾아서 연결 시켜 줌 
        UnityEngine.Object[] objects = new UnityEngine.Object[name.Length];
        _objects.Add(typeof(T), objects);

        for (int i = 0; i < name.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(gameObject, name[i], true);
            else
                objects[i] = Util.FindChild<T>(gameObject, name[i], true);
            if (objects[i] == null)
                Debug.Log($"Failed to bind({name[i]})");
        }
    }

    protected void BindObjects(Type type) { Bind<GameObject>(type); }
    protected void BindImage(Type type) { Bind<Image>(type); }
    protected void BindTexts(Type type) { Bind<TMP_Text>(type); }
    protected void BindButtons(Type type) { Bind<Button>(type); }
    protected void BindToggles(Type type) { Bind<Toggle>(type); }

    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;

        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }

    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }
    protected TMP_Text GetText(int idx) { return Get<TMP_Text>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Toggle GetToggle(int idx) { return Get<Toggle>(idx); }

    public static void BindEvent(GameObject go, Action<PointerEventData> action = null , Define.EUIEvent type = Define.EUIEvent.Click)
    {
        UI_EventHandler evt = Util.GetOrAddComponet<UI_EventHandler>(go); // 해당 오브젝트를 추가하고 넘겨줌 

        switch (type)
        {
            case Define.EUIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case Define.EUIEvent.PointerDown:
                evt.OnPointerDownHandler -= action;
                evt.OnPointerDownHandler += action;
                break;
            case Define.EUIEvent.PointerUp:
                evt.OnPointerUpHandler -= action;
                evt.OnPointerUpHandler += action;
                break;
            case Define.EUIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
        }
    }

}
