using UnityEngine;

public class GameScene : BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.EScene.GameScene;
        //TODO: 인게임 관련 스폰등 넣어주면 됨 

        return true;
    }

    public override void Clear()
    {

    }
}
