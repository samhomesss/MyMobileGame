using UnityEngine;

public class TitleScene : BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.EScene.TitleScene;
        //StartLoadAssets();

        return true;
    }

    void StartLoadAssets()
    {
        Managers.Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count}/ {totalCount}");
            if (count == totalCount)
            {
                //Managers.Scene.LoadScene();
                //Debug.Log("모든 오브젝트 다운 완료");
            }
        });
    }

    public override void Clear()
    {

    }
}
