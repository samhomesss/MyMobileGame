using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using UnityEngine;
using System;
using Object = UnityEngine.Object;


/// <summary>
/// ���ӿ� �ʿ��� ���ҽ��� �� �ε��ϰ� �޸𸮿� �ε��ϰ� ���� ���� ���°� �̻����� ���´� 
/// </summary>
public class ResourceManager
{
    Dictionary<string, Object> _resources = new Dictionary<string, Object>();
    Dictionary<string, AsyncOperationHandle> _handles = new Dictionary<string, AsyncOperationHandle>();

    #region Load Resource
    public T Load<T>(string key) where T: Object
    {
        if (_resources.TryGetValue(key , out Object resource)) // �ش� Ű ���� ������Ʈ�� ã�ƿ��� 
        {
            return resource as T; // Object Type �̴ϱ� T�� Ÿ���� ĳ���� ���ش�.
        }
        return null; // ���ٸ� null ��ȯ 
    }

    // �޸𸮿� �ε� �س��� ���͵��� �ʿ� �Ҷ����� Load �ؼ� ����ϴ� ����� ����Ѵ�.
    public GameObject Instantiate(string key, Transform parent = null , bool pooling = false)
    {
        GameObject prefab = Load<GameObject>(key);
        if (prefab == null)
        {
            Debug.LogError($"Failed to load prefab : {key}");
            return null;
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
    // �޸𸮿� ��� ������Ʈ�� ���� �ϱ� ���ؼ� Load �ϴ� �Լ� ��� 
    private void LoadAsync<T>(string key , Action<T> callback = null ) where T : UnityEngine.Object
    {
        //ĳ��
        if (_resources.TryGetValue(key, out Object resource)) // Dictionary �ȿ� ���� ���ٸ�?
        {
            callback?.Invoke(resource as T); //CallBack �� ���� �ƴҶ� ������ ���� ������Ʈ�� ���� �Ű������� �ְ� �Լ��� ����
        }

        string loadKey = key; // Ű�� �޾ƿ��� 

        if (key.Contains(".sprite")) // key �߿� .sprite ������ ã�°� 
        {
            loadKey = $"{key}[{key.Replace(".sprite", "")}]";
        }

        // sprite�� �ƴ϶�� (�Ϲ����� �����) Addressables.LoadAssetAsync<T> ��� �Լ��� �̿��ؼ� 
        // �ش� �Լ��� Handle�� ����ִµ� �� Handle���ٰ� Completed(�Ϸᰡ �Ǿ�����)�ٿ��� ���� ��ų �Լ��� �ۼ��� 
        // Handle.Completed��� �ڵ鿡 �̹� �ִ� �Լ� 
        var asyncOperation = Addressables.LoadAssetAsync<T>(loadKey);
        asyncOperation.Completed += (op) => // �Ϸᰡ �Ǿ��ٸ�
        {
            _resources.Add(key, op.Result); //  ����� ���� �ϰ� 
            _handles.Add(key, asyncOperation); // �ڵ鿡 Addressable �� ���� �ϰ� 
            callback?.Invoke(op.Result); // �ش� op ����� �Լ��� �Ѱ��� 
        };
    }

    /// <summary>
    /// ������ �����Ҷ� ��� ������Ʈ�� �о� �ö� ��� �ϴ� �� 
    /// �񵿱� �Լ� 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="label">�ش� �󺧿��ٰ� �츮�� �ε��ϰ� �;��� PreLoad �� ���� ������Ʈ��</param>
    /// <param name="callback"></param>
    public void LoadAllAsync<T>(string label , Action<string , int , int> callback) where T : Object
    {
        // �ּҸ� ������ ���ҽ��� �������� 
        var opHandle = Addressables.LoadResourceLocationsAsync(label, typeof(T)); // ������ �ּҳ� �󺧿� ���ε� ���ҽ��� ��ü ����� �ش� Ÿ������ 
                                                                                  // �񵿱�� ��ȯ �Ѵ�.
        // �ϳ��ϳ� ��ȸ �ϸ鼭 ������Ʈ���� �������°� �Ϸ� �ϴ� �� 
        opHandle.Completed += (op) =>
        {
            int loadCount = 0; // ������Ʈ �ε� 
            int totalCount = op.Result.Count;

            // ��� ������Ʈ �ܾ� �ͼ� ���� 
            foreach(var result in op.Result)
            {
                if(result.PrimaryKey.Contains(".sprite")) // Sprite�� ��� �ν��� �ȵǴ� ��찡 �־ Ư�� ��� ���ִ� �� 
                {
                    LoadAsync<Sprite>(result.PrimaryKey, (obj) =>
                    {
                        loadCount++;
                        //(result.PrimaryKey : ���������� � ������ �޾� �Դ���) , (loadCount : ���� ���� �ε��� ������ ����) , (���������� ���ҽ��� ��� �ִ���)
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
        _resources.Clear(); // ��� ���ҽ��� ����ְ� 
        foreach (var handle in _handles)
        {
            Addressables.Release(handle); // Addressable�� ��� �մ� Handle�� ���ִ� �� 
            _handles.Clear();
        }
    }

    #endregion
}
