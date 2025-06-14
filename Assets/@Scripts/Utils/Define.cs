public static class Define
{
    public enum EScene
    {
        Unknown,
        TitleScene,
        GameScene,
    }

    public enum EUIEvent
    {
        Click,
        PointerDown,
        PointerUp,
        Drag,
    }

    public enum EJoystickState
    {
        PointerDown,
        PointerUp,
        Drag,
    }

    public enum ESound
    {
        Bgm, // 일반 BGM
        Effect, // 단발성 
        Max, // 현재 ESound의 전체 갯수 
    }

    public enum EObjectType
    {
        None,
        Creature,  
        Projectile, 
        Env, // 채집물  
    }

    public enum ECreatureType
    {
        None,
        Hero,
        Monster,
        Npc,
    }

    /// <summary>
    /// Creature 상태 값
    /// </summary>
    public enum ECreatureState
    {
        None,
        Idle,
        Move,
        Skill,
        Dead,
    }
    /// <summary>
    /// Animation 이름
    /// </summary>
    public static class AnimName
    {
        public const string IDLE = "idle";
        public const string ATTACK_A = "attack_a";
        public const string ATTACK_B = "attack_b";
        public const string Move = "move";
        public const string DEAD = "dead";
    }

    public static class  SortingLayers
    {
        public const int SPELL_INDICATOR = 200;
        public const int CREATURE = 300;
        public const int ENV = 300;
        public const int PROJECTILE = 310;
        public const int SKILL_EFFECT = 310;
        public const int DAMAGE_FONT = 410;
    }
}
