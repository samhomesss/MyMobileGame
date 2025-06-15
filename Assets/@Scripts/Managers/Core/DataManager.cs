using Data;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

/// <summary>
/// 
/// </summary>
/// <typeparam name="Key">대표적인 뭔가 구분할수 있는 T (Generic) 타입</typeparam>
/// <typeparam name="Value"></typeparam>
public interface ILoader<Key , Value>
{
    Dictionary<Key, Value> MakeDict();
}
/// <summary>
/// Json 파일을 불러와서 연결 해주는 역할의 DataManager
/// DataSheet를 관리 하는 Manager
/// </summary>
public class DataManager
{
    public Dictionary<int, CreatureData> CreatureDic { get; private set; } = new Dictionary<int, CreatureData>();
    public Dictionary<int, EnvData> EnvDic { get; private set; } = new Dictionary<int, EnvData>();
    public void Init()
    {
        CreatureDic = LoadJson<CreatureDataLoader, int, CreatureData>("CreatureData").MakeDict();
        EnvDic = LoadJson<EnvDataLoader , int , EnvData>("EnvData").MakeDict();
    }

    /// <summary>
    /// TextAsset을 가져와서 TextAsset을 JsonConvert를 이용해서 Deserialize를 해주면서 
    /// 메모리상 파싱으로 해주는 것
    /// LoadJson은 어떻게 보면 모든 에셋들이 Load 되어 있는 상황에서 하는 것이 제일 좋기에 
    /// 그 과정을 먼저 해줌 
    /// </summary>
    /// <typeparam name="Loader">사실상 T ILoader<Key , Value> 를 가지고 잇는 애들 받아 오니까 MyTestDataLoader를 받아서 사용하려는 것이다.</typeparam>
    /// <typeparam name="Key">Key는 사실상 Loader에 있는 Key를 가져오는 것이다.</typeparam>
    /// <typeparam name="Value">Value는 사실상 Loaer에 있는 Value를 가져 오는 것이다 .</typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    private Loader LoadJson<Loader , Key , Value>(string path) where Loader : ILoader<Key , Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>(path); 
        return JsonConvert.DeserializeObject<Loader>(textAsset.text);
    }
}
