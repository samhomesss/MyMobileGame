using UnityEngine;

public class TitleScene : MonoBehaviour
{
    void Start()
    {
        // �񵿱� �Լ� �̱� ������ Start�� �־ �ڱⰡ �˾Ƽ� ������ �� 
        // ��� ��Ȳ�� �Ϸᰡ �Ǹ� �츮�� ���س��� Callback �Լ��� �����ϰ� �Ϸ� �뺸�� ������.
        // ��� ������ ��� �׳� �˰� �𸣰� �ڿ��� �Ǵ� ��찡 ���� �ش� �۾������� 10�� 20�� ���� ������ ���ϸ� ������ ������ �� �� �ִ�.
        Managers.Resource.LoadAllAsync<Object>("PreLoad" , (key , count , totalCount) =>
        {
            Debug.Log($"{key} {count}/{totalCount}");
            if (count == totalCount)
            {
                Debug.Log("��� ������Ʈ �ٿ� �Ϸ�");
            }
        });
    }
}
