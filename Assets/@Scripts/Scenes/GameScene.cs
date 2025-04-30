using UnityEngine;
using static Define;
public class GameScene : BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.EScene.GameScene;

        GameObject map = Managers.Resource.Instantiate("BaseMap"); // AddressAble에 올라가 있기 때문에 사용 가능 하다.
        map.transform.position = Vector3.zero;
        map.name = "@BaseMap";
        //TODO Creature 생성 

        Hero hero = Managers.Object.Spawn<Hero>(Vector3.zero);
        hero.CreatureState = ECreatureState.Move;

        Managers.UI.ShowBaseUI<UI_JoyStick>();

        return true;
    }

    public override void Clear()
    {

    }
}
