using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using UnityEngine;
using System;
using Object = UnityEngine.Object;


/// <summary>
/// 게임에 필요한 리소스를 다 로드하고 메모리에 로드하고 꺼내 쓰는 형태가 이상적인 형태다 
/// </summary>
public class ResourceManager
{
    Dictionary<string, Object> _resources = new Dictionary<string, Object>();
    Dictionary<string, AsyncOperationHandle> _handles = new Dictionary<string, AsyncOperationHandle>();

    #region Load Resource
    public T Load<T>(string key) where T: Object
    {
        if (_resources.TryGetValue(key , out Object resource)) // 해당 키 값의 오브젝트를 찾아오고 
        {
            return resource as T; // Object Type 이니까 T로 타입을 캐스팅 해준다.
        }
        return null; // 없다면 null 반환 
    }

    // 메모리에 로드 해놓은 모든것들을 필요 할때마다 Load 해서 사용하는 방식을 사용한다.
    public GameObject Instantiate(string key, Transform parent = null , bool pooling = false)
    {
        GameObject prefab = Load<GameObject>(key);
        if (prefab == null)
        {
            Debug.LogError($"Failed to load prefab : {key}");
            return null;
        }

        if (pooling)
        {
            return Managers.Pool.Pop(prefab);
        }

        GameObject go = Object.Instantiate(prefab, parent);
        go.name = prefab.name;
        return go;
    }

    public void Destory(GameObject go)
    {
        if (go == null)
            return;

        Object.Destroy(go);
    }
    #endregion

    #region Addressable
    // 메모리에 모든 오브젝트를 저장 하기 위해서 Load 하는 함수 사용 
    private void LoadAsync<T>(string key , Action<T> callback = null ) where T : UnityEngine.Object
    {
        //캐쉬
        if (_resources.TryGetValue(key, out Object resource)) // Dictionary 안에 값이 없다면?
        {
            callback?.Invoke(resource as T); //CallBack 이 널이 아닐때 위에서 나온 오브젝트의 값을 매개변수로 넣고 함수를 실행
        }

        string loadKey = key; // 키를 받아오고 

        if (key.Contains(".sprite")) // key 중에 .sprite 붙은거 찾는거 
        {
            loadKey = $"{key}[{key.Replace(".sprite", "")}]";
        }

        // sprite가 아니라면 (일반적인 경우라면) Addressables.LoadAssetAsync<T> 라는 함수를 이용해서 
        // 해당 함수가 Handle을 뱉어주는데 그 Handle에다가 Completed(완료가 되었으면)붙여서 실행 시킬 함수를 작성함 
        // Handle.Completed라면 핸들에 이미 있는 함수 
        var asyncOperation = Addressables.LoadAssetAsync<T>(loadKey);
        asyncOperation.Completed += (op) => // 완료가 되었다면
        {
            _resources.Add(key, op.Result); //  결과를 저장 하고 
            _handles.Add(key, asyncOperation); // 핸들에 Addressable 로 저장 하고 
            callback?.Invoke(op.Result); // 해당 op 결과를 함수로 넘겨줌 
        };
    }

    /// <summary>
    /// 게임을 시작할때 모든 오브젝트를 읽어 올때 사용 하는 것 
    /// 비동기 함수 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="label">해당 라벨에다가 우리가 로드하고 싶었던 PreLoad 가 붙은 오브젝트를</param>
    /// <param name="callback"></param>
    public void LoadAllAsync<T>(string label , Action<string , int , int> callback) where T : Object
    {
        // 주소를 저장한 리소스를 가져오고 
        var opHandle = Addressables.LoadResourceLocationsAsync(label, typeof(T)); // 지정된 주소나 라벨에 매핑된 리소스의 객체 목록을 해당 타입으로 
                                                                                  // 비동기로 반환 한다.
        // 하나하나 순회 하면서 오브젝트들을 가져오는걸 완료 하는 것 
        opHandle.Completed += (op) =>
        {
            int loadCount = 0; // 오브젝트 로딩 
            int totalCount = op.Result.Count;

            // 모든 오브젝트 긁어 와서 실행 
            foreach(var result in op.Result)
            {
                if(result.PrimaryKey.Contains(".sprite")) // Sprite의 경우 인식이 안되는 경우가 있어서 특별 대우 해주는 것 
                {
                    LoadAsync<Sprite>(result.PrimaryKey, (obj) =>
                    {
                        loadCount++;
                        //(result.PrimaryKey : 마지막으로 어떤 에셋을 받아 왔는지) , (loadCount : 현재 내가 로딩한 에셋의 갯수) , (최종적으로 리소스가 몇개가 있는지)
                        callback?.Invoke(result.PrimaryKey, loadCount, totalCount);
                    });
                }
                else
                {
                    LoadAsync<T>(result.PrimaryKey , (obj) => 
                    {
                        loadCount++;
                        callback?.Invoke(result.PrimaryKey, loadCount, totalCount);
                    });
                }
            }
        };
    }

    public void Clear()
    {
        _resources.Clear(); // 모든 리소스를 비워주고 
        foreach (var handle in _handles)
        {
            Addressables.Release(handle); // Addressable이 들고 잇는 Handle을 때주는 것 
            _handles.Clear();
        }
    }

    #endregion
}
