using static Define;
public class Hero : Creature
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        CreatureType = ECreatureType.Hero;
        CreatureState = ECreatureState.Idle;
        Speed = 5.0f;

        return true;
    }
}
