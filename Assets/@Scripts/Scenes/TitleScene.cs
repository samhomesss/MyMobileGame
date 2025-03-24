using UnityEngine;

public class TitleScene : MonoBehaviour
{
    void Start()
    {
        // 비동기 함수 이기 때문에 Start에 넣어도 자기가 알아서 실행이 됨 
        // 모든 상황이 완료가 되면 우리가 정해놓은 Callback 함수를 실행하고 완료 통보를 날린다.
        // 모든 배포의 경우 그냥 알게 모르게 뒤에서 되는 경우가 많지 해당 작업때문에 10초 20초 동안 게임을 못하면 불쾌한 경험이 될 수 있다.
        Managers.Resource.LoadAllAsync<Object>("PreLoad" , (key , count , totalCount) =>
        {
            Debug.Log($"{key} {count}/{totalCount}");
            if (count == totalCount)
            {
                Debug.Log("모든 오브젝트 다운 완료");
            }
        });
    }
}
