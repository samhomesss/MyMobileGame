using System;
using System.Collections.Generic;

namespace Data
{
    #region TestData 테스트용 데이터 샘플
    //[Serializable]
    //public class MyTestData // 내가 만들고 싶은 데이터 시트의 데이터를 정리 해주는 거 
    //{
    //    public int Level;
    //    public int Exp;
    //    public List<int> Skills;
    //    public float Speed;
    //    public string Name;
    //}

    ///// <summary>
    ///// 위에 있는 애를 토대로 만들어진 Loader라고 할 수 있다.
    ///// 위에 있는 애들이 여러줄이 되면 하나의 데이터 시트가 되기 때문에 그렇게 데이터 시트를 만들기 위해서
    ///// 사용하는 함수 
    ///// </summary>
    //[Serializable]
    //public class MyTestDataLoader : ILoader<int, MyTestData>
    //{

    //    public List<MyTestData> tests = new List<MyTestData>();

    //    public Dictionary<int, MyTestData> MakeDict()
    //    {
    //        Dictionary<int, MyTestData> dict = new Dictionary<int, MyTestData>();

    //        foreach (MyTestData mytestData in tests)
    //            dict.Add(mytestData.Level, mytestData); // Level이 어떻게 보면 1,2,3,4 이런식으로 ID로 사용할 수 있어서 이렇게 사용하네 

    //        return dict;
    //    }
    //}


    #endregion

    #region CreatureData
    [Serializable]
    public class CreatureData
    {
        public int DataId;
        public string DescriptionTextID;
        public string PrefabLabel;
        public float ColliderOffsetX;
        public float ColliderOffsetY;
        public float ColliderRadius;
        public float Mass;
        public float MaxHp;
        public float MaxHpBonus;
        public float Atk;
        public float AtkRange;
        public float AtkBonus;
        public float Def;
        public float MoveSpeed;
        public float TotalExp;
        public float HpRate;
        public float AtkRate;
        public float DefRate;
        public float MoveSpeedRate;
        public string SkeletonDataID;
        public string AnimatorName;
        public List<int> SkillIdList = new List<int>();
        public int DropItemId;
    }

    [Serializable]
    public class CreatureDataLoader : ILoader<int , CreatureData>
    {
        public List<CreatureData> creatures = new List<CreatureData>();

        public Dictionary<int, CreatureData> MakeDict()
        {
            Dictionary<int, CreatureData> dict = new Dictionary<int, CreatureData>();

            foreach (CreatureData creature in creatures)
                dict.Add(creature.DataId, creature);
            return dict;
        }
    }

    #endregion
}
