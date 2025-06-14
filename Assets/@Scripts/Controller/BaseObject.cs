using Spine.Unity;
using UnityEngine;
using static Define;
/// <summary>
/// 모든 오브젝트 , 몬스터 , 히어로 , 날라다니는 투사체 등 
/// 모든 오브젝트 들이 상속 받을 부모 클래스 
/// </summary>
public class BaseObject : InitBase
{
    public EObjectType ObjectType { get; protected set; } = EObjectType.None;
    public CircleCollider2D Collider { get; private set; }
    public Rigidbody2D Rigidbody { get; private set; }
    public SkeletonAnimation SkeletonAni { get; private set; } // 이건 SkeletonAnimation 버전 

    // public float ColliderRadius { get { return Collider != null ? Collider.radius : 0.0f; } }
    public float ColliderRadius { get { return Collider?.radius ?? 0.0f; } }
    public Vector3 CenterPosition { get { return transform.position + Vector3.up * ColliderRadius; } }

    public int DataTemplateID { get; set; }

    bool _lookLeft = true;

    // 온라인 멀티 게임을 염두해 두고 만든 변수 
    public bool LookLeft 
    {
        get
        { 
            return _lookLeft;
        }
        set 
        { 
            _lookLeft = value; 
            Flip(!value); 
        }
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Collider = gameObject.GetOrAddComponent<CircleCollider2D>();
        SkeletonAni = GetComponent<SkeletonAnimation>();
        Rigidbody = GetComponent<Rigidbody2D>();

        return true;
    }

    public void TranslateEx(Vector3 dir)
    {
        transform.Translate(dir);

        if (dir.x < 0)
            LookLeft = true;
        else if (dir.x > 0)
            LookLeft = false;
    }

    #region Spine 스파인 전용 함수 
    /// <summary>
    /// 상속받는 자식에서 해당 함수를 구현 하는 거 
    /// </summary>
    protected virtual void UpdateAnimation() { }
    public void PlayAnimation(int trackIndex , string AniName , bool loop)
    {
        if (SkeletonAni == null)
            return;

        SkeletonAni.AnimationState.SetAnimation(trackIndex, AniName, loop);
    }

    public void AddAnimation(int trackIndex , string AniName , bool loop , float delay)
    {
        if (SkeletonAni == null)
            return;

        SkeletonAni.AnimationState.AddAnimation(trackIndex, AniName, loop, delay);
    }

    public void Flip(bool flag)
    {
        if (SkeletonAni == null)
            return;

        SkeletonAni.Skeleton.ScaleX = flag ? -1 : 1;
    }
    #endregion
}
