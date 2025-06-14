using System.Collections;
using UnityEngine;
using static Define;
public class Creature : BaseObject
{
    public float Speed { get; protected set; } = 1.0f;
    public ECreatureType CreatureType { get; protected set; } = ECreatureType.None;
    // 상태에 따라 특정 상태에 따른 Animation 상태 관리를 해줘야 됨 
    protected ECreatureState _creatureState = ECreatureState.None;
    // 상태가 바뀌면 거기서 애니메이션 실행 
    public virtual ECreatureState CreatureState
    {
        get  { return _creatureState; }
        set
        {
            if (_creatureState != value)
            {
                _creatureState = value;
                UpdateAnimation();
            }
        }
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = EObjectType.Creature;
        CreatureState = ECreatureState.Idle;
        return true;
    }

    protected override void UpdateAnimation()
    {
        switch (CreatureState)
        {
            case ECreatureState.Idle:
                PlayAnimation(0, AnimName.IDLE, true);
                break;
            case ECreatureState.Move:
                PlayAnimation(0, AnimName.Move, true);
                break;
            case ECreatureState.Skill:
                PlayAnimation(0, AnimName.ATTACK_A, true);
                break;
            case ECreatureState.Dead:
                PlayAnimation(0, AnimName.DEAD, true);
                Rigidbody.simulated = false;
                break;
            default:
                break;
        }
    }


    #region AI
    /// <summary>
    /// 프레임 관리 측면에서 이게 맞는지 고민 해야됨 
    /// -> 이때문에 Corutain을 사용해서 작업 
    /// </summary>
    //private void Update()
    //{
    //    switch (CreatureState)
    //    {
    //        case ECreatureState.Idle:
    //            UpdateIdle();
    //            break;
    //        case ECreatureState.Move:
    //            UpdateMove();
    //            break;
    //        case ECreatureState.Skill:
    //            UpdateSkill();
    //            break;
    //        case ECreatureState.Dead:
    //            UpdateDead();
    //            break;
    //        default:
    //            break;
    //    }
    //}
    public float UpdateAITick { get; protected set; } = 0.0f;
    protected IEnumerator CoUpdateAI()
    {
        while (true)
        {
            switch (CreatureState) // 원래는 이벤트 관리를 통해서 어떤 이벤트를 실행할지를 하면 된다고 생각하긴 함
            {
                case ECreatureState.Idle:
                    UpdateIdle();
                    break;
                case ECreatureState.Move:
                    UpdateMove();
                    break;
                case ECreatureState.Skill:
                    UpdateSkill();
                    break;
                case ECreatureState.Dead:
                    UpdateDead();
                    break;
                default:
                    break;
            }

            if (UpdateAITick > 0) // 주기 조절 , 상황에 따라 조절 할 수있기에 매 프레임은 아니도록 만들 수 있다.
                yield return new WaitForSeconds(UpdateAITick);
            else
                yield return null;
        }
    }

    protected virtual void UpdateIdle() { }
    protected virtual void UpdateMove() { }
    protected virtual void UpdateSkill() { }
    protected virtual void UpdateDead() { }
    #endregion

    #region Wait - 얼마나 기다릴지에 대한 Wait 계산
    protected Coroutine _coWait; // 해당이 끝이나면 실행 아니면 실행 안함

    protected void StartWait(float second)
    {
        CancelWait();
        _coWait = StartCoroutine(CoWait(second));
    }

    IEnumerator CoWait (float second)
    {
        yield return new WaitForSeconds(second);
        _coWait = null;
    }

    protected void CancelWait()
    {
        if (_coWait != null)
            StopCoroutine(_coWait);

        _coWait = null;
    }
    #endregion
}
