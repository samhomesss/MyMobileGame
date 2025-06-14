using UnityEngine;
using static Define;

public class Monster : Creature
{
    public override ECreatureState CreatureState 
    {
        get { return base.CreatureState; }
        set
        {
            if (_creatureState != value)
            {
                base.CreatureState = value; // base에 있는 CreatrueState를 실행시켜서 Animation을 실행 시킴 
                switch (value)
                {
                    case ECreatureState.Idle:
                        UpdateAITick = 0.5f;
                        break;
                    case ECreatureState.Move:
                        UpdateAITick = 0.0f;
                        break;
                    case ECreatureState.Skill:
                        UpdateAITick = 0.0f;
                        break;
                    case ECreatureState.Dead:
                        UpdateAITick = 1.0f;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        CreatureType = ECreatureType.Monster;
        CreatureState = ECreatureState.Idle;
        Speed = 3.0f;

        StartCoroutine(CoUpdateAI());

        return true;
    }

    private void Start()
    {
        _initPos = transform.position;
    }

    #region AI
    public float SearchDistance { get; private set; } = 8.0f; // 확인 사거리 
    public float AttackDistance { get; private set; } = 4.0f; // 공격 사거리

    Creature _target; // target
    Vector3 _destPos; // 정찰 위치 
    Vector3 _initPos; // 원래 위치

    protected override void UpdateIdle()
    {
        Debug.Log("Idle");

        // Patrol
        {
            int patrolPercent = 10;
            int rand = Random.Range(0, 100);

            if (rand <= patrolPercent)
            {
                _destPos = _initPos + new Vector3(Random.Range(-2, 2), Random.Range(-2, 2));
                CreatureState = ECreatureState.Move;
                return;
            }
        }


        // Search Player
        {
            Creature target = null;
            float bestDistanceSqr = float.MaxValue;
            float searchDistanceSqr = SearchDistance * SearchDistance; // 연산에 제곱을 쓴 이유는 이게 더 최적화에 맞기 때문이다.

            foreach (Hero hero in Managers.Object.Heros)
            {
                Vector3 dir = hero.transform.position - transform.position;
                float disToTargetSqr = dir.sqrMagnitude;

                Debug.Log(disToTargetSqr);

                if (disToTargetSqr > searchDistanceSqr)
                    continue; // 너무 멀리 있으면 스킵 
                if (disToTargetSqr > bestDistanceSqr)
                    continue; // 더 좋은 거리를 찾았다면 이것도 스킵 

                target = hero;
                bestDistanceSqr = disToTargetSqr;
            }

            _target = target;

            if (_target != null)
                CreatureState = ECreatureState.Move;
        }

    }
    protected override void UpdateMove()
    {
        if (_target == null) // 기본적인 이동 상태 
        {
            // Patrol or Return
            Vector3 dir = (_destPos - transform.position);
            float moveDist = Mathf.Min(dir.magnitude, Time.deltaTime * Speed);
            transform.TranslateEx(dir.normalized * moveDist);

            if (dir.sqrMagnitude <= 0.01f)
                CreatureState = ECreatureState.Idle;
        }
        else 
        {
            //Chase
            {
                Vector3 dir = (_target.transform.position - transform.position);
                float distToTargetSqr = dir.sqrMagnitude;
                float attackDistanceSqr = AttackDistance * AttackDistance;

                if (distToTargetSqr < attackDistanceSqr)
                {
                    // 공격 범위 이내로 들어오면 공격
                    CreatureState = ECreatureState.Skill;
                    StartWait(2.0f);
                }
                else
                {
                    // 공격 범위 밖이라면  추척
                    float moveDist = Mathf.Min(dir.magnitude, Time.deltaTime * Speed); // Tick 크기만큼만 이동 하도록 설정 
                    transform.TranslateEx(dir.normalized * moveDist);

                    float searchDistanceSqr = SearchDistance * SearchDistance;
                    if (distToTargetSqr > searchDistanceSqr)
                    {
                        _destPos = _initPos;
                        _target = null;
                        CreatureState = ECreatureState.Move;
                    }
                }
            }
        }
    }
    protected override void UpdateSkill()
    {
        // 일정 시간이 지난후에 해당 Move로 방향을 바꿔줌 
        if (_coWait != null)
            return;

        CreatureState = ECreatureState.Move;
    }
    protected override void UpdateDead()
    {

    }

    #endregion
}
