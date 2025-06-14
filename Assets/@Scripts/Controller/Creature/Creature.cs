using Data;
using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using static Define;
public class Creature : BaseObject
{
    public float Speed { get; protected set; } = 1.0f; 
    public CreatureData CreatureData { get; private set; } // 데이터 시트 를 가져와서 어떤 Creature 인지 확인 
    public ECreatureType CreatureType { get; protected set; } = ECreatureType.None;

    #region Stats : Json 파일 파싱해서 가져온 데이터 넣는 부분
    public float Hp { get; set; }
    public float MaxHP { get; set; }
    public float MaxHpBonusRate { get; set; }
    public float HealBonusRate { get; set; }
    public float HpRegen{ get; set; }
    public float Atk { get; set; }
    public float AttackRate { get; set; }
    public float Def { get; set; }
    public float DefRate { get; set; }
    public float CriRate { get; set; }
    public float CriDamage { get; set; }
    public float DamageReduction { get; set; }
    public float MoveSpeedRate { get; set; }
    public float MoveSpeed { get; set; }
    #endregion


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
        //CreatureState = ECreatureState.Idle;
        return true;
    }

    /// <summary>
    /// 현재 데이터 시트의 ID가 무엇인지에 따라 Creature 세팅하는 것 
    /// </summary>
    /// <param name="templateID"></param>
    public virtual void SetInfo(int templateID)
    {
        DataTemplateID = templateID; // 현재 아이디 넣어주고 

        CreatureData = Managers.Data.CreatureDic[templateID]; // 해당 아이디에 따른 데이터 긁어오고 
        gameObject.name = $"{CreatureData.DataId}_{CreatureData.DescriptionTextID}"; // 관리하기 쉽도록 해당 오브젝트의 이름을 지정해줌

        // Collider
        Collider.offset = new Vector2(CreatureData.ColliderOffsetX, CreatureData.ColliderOffsetY);
        Collider.radius = CreatureData.ColliderRadius;

        //RigidBody
        Rigidbody.mass = CreatureData.Mass;

        //Spine : 이부분이 바뀌게 되면 Skeleton에 따라서 교체가 될 것이기 때문에 다양한 몬스터가 나오게 될 것 
        // TODO : Spine 뿐만이 아니라 Animation 또한 해당 기능이 있었던걸로 알기에 한번 나중에 따로 공부 해 봐야 함 
        SkeletonAni.skeletonDataAsset = Managers.Resource.Load<SkeletonDataAsset>(CreatureData.SkeletonDataID);
        SkeletonAni.Initialize(true);

        //Spine SkeleltonAnimation은 SpriteRenderer 를 사용하지 않고 MashRenderer를 사용함
        // 그렇기 때문에 2D Sort Axis가 안 먹히게 되는데 SortingGroup을 SpriteRenderer , MashRenderer를 같이 계산함
        SortingGroup sg = Util.GetOrAddComponet<SortingGroup>(gameObject);
        sg.sortingOrder = SortingLayers.CREATURE;

        //Skill
        //CreatureData.SkillIdList;

        //Stat
        MaxHP = CreatureData.MaxHp;
        Hp = CreatureData.MaxHp;
        Atk = CreatureData.MaxHp;
        MaxHP = CreatureData.MaxHp;
        MoveSpeed = CreatureData.MoveSpeed;

        //State : 원래 Init에서 설정하던 부분이 내려오게 됨 Spine 데이터를 읽고 애니메이션을 실행 시켜야 하는데 
        // 원래 있는 코드에서는 그 과정이 없기 때문에 애니메이션을 찾아 오지 못함.
        CreatureState = ECreatureState.Idle;

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
