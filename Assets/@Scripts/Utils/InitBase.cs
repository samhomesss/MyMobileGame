using UnityEngine;

/// <summary>
/// Scene에서 만약 매번 초기화 되었는지 명명 해주면 코드 자체가 더러워 질 가능성이 높기 때문에 
/// 해당 방식을 이용해서 코드가 더러워 지는걸 방지 하려 한다.
/// </summary>
public class InitBase : MonoBehaviour
{
    protected bool _init = false;

    public virtual bool Init()
    {
        if (_init)
            return false;

        _init = true;
        return true;
    }

    private void Awake()
    {
        Init();
    }
}
