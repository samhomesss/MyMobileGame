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

    public static T ParseEnum<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }
}
