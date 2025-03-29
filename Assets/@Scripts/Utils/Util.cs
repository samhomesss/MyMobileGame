using System;
using UnityEngine;

public static class Util
{
    public static T GetOrAddComponet<T>(GameObject go) where T : UnityEngine.Component
    {
        T componet = go.GetComponent <T>();

        if (componet == null)
            componet = go.AddComponent<T>();
        return componet;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;
        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);

                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }

        else
        {
            foreach(T componet in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || componet.name == name)
                {
                    return componet;
                }
            }
        }

        return null;

    }

    /// <summary>
    /// Enum.Parse 메서드를 호출해서 문자열 value를 enum 타입으로 변환합니다.
    /// 이 함수는 문자열 값을 제네릭 타입의 enum으로 변환하는 데 사용됩니다. 대소문자 구분 없이 문자열을 받아 enum 값으로 변환해주기 때문에,
    /// 코드의 유연성을 높이고 쉽게 enum 값을 파싱할 수 있도록 도와줍니다.
    /// 예를 들어, enum 타입 Color가 있고, ParseEnum<Color>("Red")를 호출하면 Color.Red를 반환하게 됩니다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static T ParseEnum<T>(string value)
    {
        /// typeof(T) -> 반환할 Enum의 타입
        /// value -> 반환하고자 하는 문자열 
        /// true -> 대소문자를 구분하지 않고 파싱하도록 지정 , 즉 value의 대소문자에 관계없이 enum값을 찾음
        /// 파싱된 결과를 T 로 형 변환 해서 반호나 
        return (T)Enum.Parse(typeof(T), value, true);
    }
}
