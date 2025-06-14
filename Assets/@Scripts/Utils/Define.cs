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


    //임시로 각 데이터에 대한 TemplateID 설정 
    public const int CAMERA_PROJECTION_SIZE = 12;

    public const int HERO_WIZARD_ID = 201000;
    public const int HERO_KNIGHT_ID = 201001;

    public const int MONSTER_SLIME_ID = 202001;
    public const int MONSTER_SPIDER_COMMON_ID = 202002;
    public const int MONSTER_WOOD_COMMON_ID = 202004;
    public const int MONSTER_GOBLIN_ARCHER_ID = 202005;
    public const int MONSTER_BEAR_ID = 202006;

    public const int ENV_TREE1_ID = 300001;
    public const int ENV_TREE2_ID = 301000;




    /// <summary>
    /// Animation 이름
    /// Attack_a 와 Attack_b 처럼 동일한 이름이 있어도 되는 것인가?
    /// </summary>
    public static class AnimName
    {
        public const string ATTACK_A = "attack";
        public const string ATTACK_B = "attack";
        public const string SKILL_A = "skill";
        public const string SKILL_B = "skill";
        public const string IDLE = "idle";
        public const string Move = "move";
        public const string DAMAGED = "hit";
        public const string DEAD = "dead";
        public const string EVENT_ATTACK_A = "event_attack";
        public const string EVENT_ATTACK_B = "event_attack";
        public const string EVENT_SKILL_A = "event_attack";
        public const string EVENT_SKILL_B = "event_attack";
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
