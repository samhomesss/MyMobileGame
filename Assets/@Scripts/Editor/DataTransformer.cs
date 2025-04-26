using System.Collections.Generic;
using UnityEditor;
using System.IO;
using UnityEngine;
using System.Linq;
using Unity.Plastic.Newtonsoft.Json;
using Data;
using System;
using System.Reflection;
using System.Collections;
using System.ComponentModel;

/// <summary>
/// Excel 기반으로 되어 있는 데이터 시트를 Json으로 파싱해서 바꿔주는 역할을 하는 Tool
/// 리픞랙션을 이용 
/// Type type = typeof(TestDataLoader) 이런식으로 사용하는 것 
/// type.GetFields -> 타입을 얻어 오는 것 
/// 해당 코드는 내가 깊이 있는 공부가 필요 할 듯?
/// </summary>
public class DataTransformer : EditorWindow
{
#if UNITY_EDITOR 
    [MenuItem("Tools/ParseExcel %#K")] 
    public static void ParseExcelDataToJson()
    {
        ParseExcelDataToJson<MyTestDataLoader, MyTestData>("Test");

        Debug.Log("DataTransformer Completed");
    }

    #region LEGACY
    public static T ConvertValue<T>(string value)
    {
        if (string.IsNullOrEmpty(value))
            return default(T);

        TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
        return (T)converter.ConvertFromString(value);
    }

    public static List<T> ConvertList<T>(string value)
    {
        if (string.IsNullOrEmpty(value))
            return new List<T>();

        return value.Split('&').Select(x => ConvertValue<T>(x)).ToList();
    }

    static void LEGACY_ParseTestData(string filename)
    {
        MyTestDataLoader loader = new MyTestDataLoader();

        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/ExcelData/{filename}Data.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');
            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;

            int i = 0;
            MyTestData testData = new MyTestData();
            testData.Level = ConvertValue<int>(row[i++]);
            testData.Exp = ConvertValue<int>(row[i++]);
            testData.Skills = ConvertList<int>(row[i++]);
            testData.Speed = ConvertValue<float>(row[i++]);
            testData.Name = ConvertValue<string>(row[i++]);

            loader.tests.Add(testData);
        }

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
        AssetDatabase.Refresh();
    }
    #endregion

    #region Helpers
    private static void ParseExcelDataToJson<Loader, LoaderData>(string filename) where Loader : new() where LoaderData : new()
    {
        Loader loader = new Loader();
        FieldInfo field = loader.GetType().GetFields()[0];
        field.SetValue(loader, ParseExcelDataToList<LoaderData>(filename));

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
        AssetDatabase.Refresh();
    }

    private static List<LoaderData> ParseExcelDataToList<LoaderData>(string filename) where LoaderData : new()
    {
        List<LoaderData> loaderDatas = new List<LoaderData>();

        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/ExcelData/{filename}Data.csv").Split("\n");

        for (int l = 1; l < lines.Length; l++)
        {
            string[] row = lines[l].Replace("\r", "").Split(',');
            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;

            LoaderData loaderData = new LoaderData();

            System.Reflection.FieldInfo[] fields = typeof(LoaderData).GetFields();
            for (int f = 0; f < fields.Length; f++)
            {
                FieldInfo field = loaderData.GetType().GetField(fields[f].Name);
                Type type = field.FieldType;

                if (type.IsGenericType) 
                {
                    object value = ConvertList(row[f], type);
                    field.SetValue(loaderData, value); 
                }
                else // 그게 아니면 벨류를 호출한다.
                {
                    object value = ConvertValue(row[f], field.FieldType);
                    field.SetValue(loaderData, value);
                }
            }

            loaderDatas.Add(loaderData);
        }

        return loaderDatas;
    }

    private static object ConvertValue(string value, Type type)
    {
        if (string.IsNullOrEmpty(value))
            return null;

        TypeConverter converter = TypeDescriptor.GetConverter(type);
        return converter.ConvertFromString(value);
    }

    private static object ConvertList(string value, Type type)
    {
        if (string.IsNullOrEmpty(value))
            return null;

  
        Type valueType = type.GetGenericArguments()[0]; 
        Type genericListType = typeof(List<>).MakeGenericType(valueType); 
        
        var genericList = Activator.CreateInstance(genericListType) as IList;

        var list = value.Split('&').Select(x => ConvertValue(x, valueType)).ToList();

        foreach (var item in list)
            genericList.Add(item);

        return genericList;
    }
    #endregion

#endif
}
