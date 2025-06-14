using Unity.VisualScripting;
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

        Hero hero = Managers.Object.Spawn<Hero>(new Vector3Int(-10, -5, 0));
        hero.CreatureState = ECreatureState.Idle;

        CameraController camera = Camera.main.GetOrAddComponent<CameraController>();
        camera.Target = hero;

        Managers.UI.ShowBaseUI<UI_JoyStick>();

        Monster monster = Managers.Object.Spawn<Monster>(new Vector3Int(0, 1, 0));
        monster.CreatureState = ECreatureState.Idle;

        return true;
    }

    public override void Clear()
    {

    }
}
