using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

internal class Pool
{
    GameObject _prefab; // 생성할 프리팹
    IObjectPool<GameObject> _pool; // 풀링 

    Transform _root; // 오브젝트 넣어줄 부모 오브젝트의 위치 
    Transform Root
    {
        get
        {
            if (_root == null)
            {
                GameObject go = new GameObject() { name = $"@{_prefab.name}Pool" };
                _root = go.transform;
            }

            return _root;
        }
    }

    public Pool(GameObject prefab)
    {
        _prefab = prefab;
        _pool = new ObjectPool<GameObject>(OnCreate, OnGet, OnRelease, OnDestroy);
    }

    public void Push(GameObject go)
    {
        if (go.activeSelf)
            _pool.Release(go);
    }

    public GameObject Pop()
    {
        return _pool.Get();
    }

    GameObject OnCreate()
    {
        GameObject go = GameObject.Instantiate(_prefab);
        go.transform.SetParent(Root);
        go.name = _prefab.name;
        return go;
    }

    void OnGet(GameObject go)
    {
        go.SetActive(true);
    }

    void OnRelease(GameObject go)
    {
        go.SetActive(false);
    }

    void OnDestroy(GameObject go)
    {
        GameObject.Destroy(go);
    }
}

public class PoolManager
{
    private Dictionary<string, Pool> _pools = new Dictionary<string, Pool>();

    /// <summary>
    /// 저장소에서 꺼내옴 해당 이름을 가진 것들을 Pool에서 찾아서 있다면 가져옴 
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public GameObject Pop(GameObject prefab)
    {
        if (_pools.ContainsKey(prefab.name) == false)   
            CreatePool(prefab);

        return _pools[prefab.name].Pop();
    }

    /// <summary>
    /// Push는 반납을 하는 것 오브젝트 풀링의 개념이 삭제의 개념이 아니라 사용이 끝났으면 삭제 하지 않고 다시 원상 복구 시켜놓는 개념
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    public bool Push(GameObject go)
    {
        if (_pools.ContainsKey(go.name) == false) // 그래서 위에 설명을 이어서 한다면 반납 하는것이 해당 _pool 이라는 Dictionary에서
            // 찾았는데 만약에 없다면 해당 오브젝트는 한번도 생성된적이 없는 오브젝트기 때문에 반납이 불가하다 그래서 false 반환 
        {
            return false;
        }

        _pools[go.name].Push(go); // 그게 아니라면 해당 이름을 찾아서 Pool에 있는 push 를 적용 시켜 주는 것이다.
        return true;
    }

    /// <summary>
    /// 해당 Pool을 지워서 초기화 하는 것 
    /// </summary>
    public void Clear()
    {
        _pools.Clear();
    }

    private void CreatePool(GameObject original)
    {
        Pool pool = new Pool(original);
        _pools.Add(original.name, pool);
    }

}
