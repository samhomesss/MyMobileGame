using Data;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public interface ILoader<Key , Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<int, MyTestData> MyTestDic { get; private set; } = new Dictionary<int, MyTestData>();

    public void Init()
    {
        MyTestDic = LoadJson<MyTestDataLoader, int, MyTestData>("TestData").MakeDict();
    }

    private Loader LoadJson<Loader , Key , Value>(string path) where Loader : ILoader<Key , Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>(path); 
        return JsonConvert.DeserializeObject<Loader>(textAsset.text);
    }
}
