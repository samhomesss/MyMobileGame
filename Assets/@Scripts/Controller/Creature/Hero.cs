using UnityEngine;
using static Define;
public class Hero : Creature
{
    Vector2 _moveDir = Vector2.zero;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        CreatureType = ECreatureType.Hero;
        CreatureState = ECreatureState.Idle;
        Speed = 5.0f;

        Managers.Game.OnMoveDirChanged -= HandleOnMoveDirChanged;
        Managers.Game.OnMoveDirChanged += HandleOnMoveDirChanged;
        Managers.Game.OnJoyStickStateChanged -= HandleOnJoystickStateChanged;
        Managers.Game.OnJoyStickStateChanged += HandleOnJoystickStateChanged;

        return true;
    }

    private void Update()
    {
        transform.TranslateEx(_moveDir * Time.deltaTime * Speed);
    }

    private void HandleOnMoveDirChanged(Vector2 dir)
    {
        _moveDir = dir;

        Debug.Log(dir);
    }

    private void HandleOnJoystickStateChanged(EJoystickState joystickState)
    {
        switch(joystickState)
        {
            case EJoystickState.PointerDown:
                CreatureState = ECreatureState.Move;
                break;
            case EJoystickState.Drag:
                break;
            case EJoystickState.PointerUp:
                CreatureState = ECreatureState.Idle;
                break;
        }
    }
}
