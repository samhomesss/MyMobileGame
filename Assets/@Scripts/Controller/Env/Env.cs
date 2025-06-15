using Data;
using UnityEngine;
using static Define;

public class Env : BaseObject
{
    private EnvData _data; // 현재 내가 어떤 데이터를 들고 있는지 

    private EEnvState _envState = Define.EEnvState.Idle;
    public EEnvState EnvState
    {
        get => _envState;
        set
        {
            _envState = value;
            UpdateAnimation();
        }
    }

    #region State
    public float Hp { get; set; }
    public float MaxHp { get; set; }
    #endregion

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = EObjectType.Env;

        return true;
    }


    public void SetInfo(int templateID)
    {
        DataTemplateID = templateID;
        _data = Managers.Data.EnvDic[templateID]; // 해당 데이터 시트의 경우 게임이 시작 된 후에 아무 변동이 없을 것이기 때문에 그냥 들고 있어도 됨 

        //Stat
        Hp = _data.MaxHp;
        MaxHp = _data.MaxHp;

        //Spine
        string ranSpine = _data.SkeletonDataIDs[Random.Range(0, _data.SkeletonDataIDs.Count)];
        SetSpineAnimation(ranSpine , SortingLayers.ENV);
    }

    /// <summary>
    /// 환경 채집물의 경우 완전히 다른 Animation을 사용하기 때문에 해당 생물만의 애니메이션을 만듬 
    /// </summary>
    protected override void UpdateAnimation()
    {
        switch (EnvState)
        {
            case EEnvState.Idle:
                PlayAnimation(0, AnimName.IDLE, true);
                break;
            case EEnvState.OnDamaged:
                PlayAnimation(0, AnimName.DAMAGED, true);
                break;
            case EEnvState.Dead:
                PlayAnimation(0, AnimName.DEAD, true);
                break;
            default:
                break;
        }
    }
}
