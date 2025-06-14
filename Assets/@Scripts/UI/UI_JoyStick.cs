using UnityEngine;
using UnityEngine.EventSystems;

public class UI_JoyStick : UI_Base
{
    enum GameObjects
    {
        JoystickBG,
        JoystickCursor,
    }

    GameObject _backGround;
    GameObject _cursor;
    float _radius;
    Vector2 _touchPos;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObjects(typeof(GameObjects));
        _backGround = GetObject((int)GameObjects.JoystickBG);
        _cursor = GetObject((int)GameObjects.JoystickCursor);
        _radius = _backGround.GetComponent<RectTransform>().sizeDelta.y / 5;

        gameObject.BindEvent(OnPointerDown, type: Define.EUIEvent.PointerDown); // 사실상 이부분을 InputSystem에서 해주는 거 
        gameObject.BindEvent(OnPointerUp, type: Define.EUIEvent.PointerUp);
        gameObject.BindEvent(OnDrag, type: Define.EUIEvent.Drag);

        return true;
    }

    #region InputSystem
    // Move
    // Jump -> EJumpInput  -> Action이 넘어가면 Animation 실행 
    // Input -> Dir 

    #endregion

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventData"> 내가 누르는 자리 </param>
    public void OnPointerDown(PointerEventData eventData)
    {
        _backGround.transform.position = eventData.position;
        _cursor.transform.position = eventData.position;
        _touchPos = eventData.position;
        Managers.Game.JoyStickState = Define.EJoystickState.PointerDown;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        _cursor.transform.position = _touchPos;
        Managers.Game.MoveDir = Vector2.zero;
        Managers.Game.JoyStickState = Define.EJoystickState.PointerUp;
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 touchDir = (eventData.position - _touchPos);

        float moveDist = Mathf.Min(touchDir.magnitude, _radius);
        Vector2 moveDir = touchDir.normalized;
        Vector2 newPosition = _touchPos + moveDir * moveDist;
        _cursor.transform.position = newPosition;

        Managers.Game.MoveDir = moveDir;
        Managers.Game.JoyStickState = Define.EJoystickState.Drag;
    }




}
