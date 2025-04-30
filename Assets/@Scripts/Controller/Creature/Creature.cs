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

}
