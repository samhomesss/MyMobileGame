using UnityEngine;
using static Define;
public class UI_TitleScene : UI_Scene
{
    enum GameObjects
    {
        StartImage
    }

    enum Texts
    {
        DisplayText
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObjects(typeof(GameObjects));
        BindTexts(typeof(Texts));

        GetObject((int)GameObjects.StartImage).BindEvent((evt) =>
        {
            Debug.Log("ChangeScene");
            Managers.Scene.LoadScene(EScene.GameScene);
        });

        GetObject((int)GameObjects.StartImage).gameObject.SetActive(false);
        GetText((int)Texts.DisplayText).text = $"";

        StartLoadAssets();

        return true;
    }


    /// <summary>
    /// 게임 시작전 Addressable에 들어가 있는 오브젝트를 로드 하기 위함 
    /// </summary>
    void StartLoadAssets()
    {
        Managers.Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count}/ {totalCount}");
            if (count == totalCount)
            {
                Managers.Data.Init(); // 로드 할때 Data초기화도 같이 해줌 

                GetObject((int)GameObjects.StartImage).gameObject.SetActive(true);
                GetText((int)Texts.DisplayText).text = $"Touch To Start";

                //Managers.Data.MyTestDic[1]; 이런식으로 사용가능하다
            }
        });
    }

}
