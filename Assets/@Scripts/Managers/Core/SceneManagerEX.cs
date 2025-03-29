using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEX 
{
    public BaseScene CurrentScene { get { return GameObject.FindAnyObjectByType<BaseScene>(); } }

    public void LoadScene(Define.EScene type)
    {
        SceneManager.LoadScene(GetSceneName(type));
    }
    
    string GetSceneName(Define.EScene type)
    {
        string name = System.Enum.GetName(typeof(Define.EScene), type);
        return name;
    }

    public void Clear()
    {
        //CurrentScene.Clear();
    }
}
