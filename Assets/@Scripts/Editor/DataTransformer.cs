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
#if UNITY_EDITOR // 때때로 Unity_Editor에서만 사용할 수 있는 기능이라고 동작 중에 에러가 뜨는 경우가 있어 #if Unity_Editor를 붙여줌 
    /// <summary>
    /// ctrl + shift + k 를 누르면 단축키가 발생 하도록 만듬 
    /// </summary>
    [MenuItem("Tools/ParseExcel %#K")] // 위에 상단쪽에 메뉴에 TOOL 이라는 도구창으로 이용 가능 하도록 만들고  % -> Ctrl # -> Shift 
    public static void ParseExcelDataToJson()
    {
        // 내가 읽어 오고 싶은 데이터의 데이터 형태와  파일 이름으로 Excel 데이터를 Json으로 변환 
        //TestData 클래스를 읽어올 TestDataLoder , 내가 읽어 오고 싶은 Data class (TestData) , 그리고 Data 부분을 뺀 Excel의 이름 
        ParseExcelDataToJson<MyTestDataLoader, MyTestData>("Test");
        //LEGACY_ParseTestData("Test");

        Debug.Log("DataTransformer Completed");
    }

    #region LEGACY
    // LEGACY !
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
            //-> Field를 하나씩 다 긁어 와서 하나씩 확인을 하는데 
            for (int f = 0; f < fields.Length; f++)
            {
                //Field 방식으로 가져와서 Reflection 기능을 이용해서 사용한다.
                FieldInfo field = loaderData.GetType().GetField(fields[f].Name);
                Type type = field.FieldType;

                if (type.IsGenericType) // 해당 필드의 Type이 GenericType 이면 리스트를 호출하고 
                {
                    object value = ConvertList(row[f], type);
                    field.SetValue(loaderData, value); // 그리고 받아온 값을 이용해서 Setting 해줌 
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

        // Reflection
        Type valueType = type.GetGenericArguments()[0]; //첫번째 인자를 가져오고 
        Type genericListType = typeof(List<>).MakeGenericType(valueType); // 가져와서 리스트로 만듬 
        // TODO : 여기서 public List<MyTestData> tests = new List<TestData>를 만들고 싶은 것 
        var genericList = Activator.CreateInstance(genericListType) as IList;

        // Parse Excel
        var list = value.Split('&').Select(x => ConvertValue(x, valueType)).ToList();

        foreach (var item in list)
            genericList.Add(item);

        return genericList;
    }
    #endregion

#endif
}
