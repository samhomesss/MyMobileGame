using UnityEngine;

public class UI_Scene : UI_Base
{
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        
        return true;
    }
}
