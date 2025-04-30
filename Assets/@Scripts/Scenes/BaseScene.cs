using UnityEngine;
using UnityEngine.EventSystems;
using static Define;
/// <summary>
/// 모든 Scene들의 부모 역할을 담당하는 스크립트
/// </summary>
public abstract class BaseScene : InitBase
{
    // 해당 씬 타입을 설정 할 수 있도록 부모 오브젝트에 넣어주고 해당 씬 타입을 나중에 자식에서 설정 할 수 있도록 해달라 
    public EScene SceneType { get; protected set; } = Define.EScene.Unknown;
    // 상속을 받아서 코드를 넣을때 기본 예약된 함수들의 경우 일종의 가상 함수 라고 할 수 있다.
    // 만약 Awake나 Start등 그런 함수들을 자식에서 사용하지 않았다면 상위 계층에 있는 부모의 Start 나 Awake가 동작하고
    // 그게 아니라면 자식에 있는 Start와 Awake가 실행이 된다.

    // 만약 자식에서 다른 부분들을 초기화 해야 한다면 해당 Init을 가지고 초기화 할수 있다.
    // 원래는 부모에서 사용하는 것이 아닌 자식에 전달하는 Init이었는데 해당 Init을 InitBase에서 가져와서 사용하다 보니 
    // override로 명명 규칙 변경
    public override bool Init()
    {
        if (base.Init() == false) // 현재 Init의 경우 부모에서 초기화 되었는지 확인 할 수 있으니 해당 베이스의 Init이 초기화 되었는지 확인함 
        {
            return false;
        }

        //_init = true; // InitBase에 있는 _init을  true로 바꿔주면서 값을 초기화 해주었다고 지정 해줌
        // 모든 씬에는 EventSystem이 필요하는 경우가 많다. 없다면 생성 해주자 
        Object obj = GameObject.FindAnyObjectByType(typeof(EventSystem));
        if (obj == null)
        {
            GameObject go = new GameObject { name = "@EventSystem" };
            go.AddComponent<EventSystem>();
            go.AddComponent<StandaloneInputModule>(); // Spine을 움직이기 위한 방식 
        }

     
        return true;
    }

    // 추후에 각 씬들이 지워질때 어떤 식으로 될지 말하는 거 
    public abstract void Clear();
}
