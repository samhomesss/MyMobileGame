using System;
using System.Collections.Generic;

namespace Data
{
    #region TestData
    [Serializable]
    public class MyTestData 
    {
        public int Level;
        public int Exp;
        public List<int> Skills;
        public float Speed;
        public string Name;
    }

    [Serializable]
    public class MyTestDataLoader : ILoader<int, MyTestData>
    {

        public List<MyTestData> tests = new List<MyTestData>();

        public Dictionary<int, MyTestData> MakeDict()
        {
            Dictionary<int, MyTestData> dict = new Dictionary<int, MyTestData>();

            foreach (MyTestData mytestData in tests)
                dict.Add(mytestData.Level, mytestData);

            return dict;
        }
    }


    #endregion
}
