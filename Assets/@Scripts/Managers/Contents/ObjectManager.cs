using Data;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ObjectManager 
{
    /// <summary>
    /// 싱글 게임일 경우 List로 만드는게 더 좋을 수 있으나 온라인 게임으로의 확장성 고려 
    /// </summary>
    public HashSet<Hero> Heros { get; } =  new HashSet<Hero>();
    public HashSet<Monster> Monsters { get; } = new HashSet<Monster>();
    public HashSet<Env> Envs { get; } = new HashSet<Env>(); // 채집물 저장할 HashSet

    #region Root
    public Transform GetRootTransform(string name)
    {
        GameObject root = GameObject.Find(name);
        if (root == null)
            root = new GameObject { name = name };

        return root.transform;
    }

    public Transform HeroRoot { get { return GetRootTransform("@Hero"); } }
    public Transform MonsterRoot { get { return GetRootTransform("@Monster"); } }
    public Transform EnvRoot { get { return GetRootTransform("@Env"); } }
    #endregion

    /// <summary>
    /// 이제 Spawn 해줄 때 해당 아이디를 가지고 Spawn 해주면 됨 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="position"></param>
    /// <returns></returns>
    public T Spawn<T>(Vector3 position , int templateID ) where T : BaseObject
    {
        string prefabName = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate(prefabName);
        go.name = prefabName;
        go.transform.position = position;

        BaseObject obj = go.GetComponent<BaseObject>();
        //밑에 방법으로 진행해서 creature 자리에 넣어줘도 됨 
        //Creature c = obj as Creature;

        if (obj.ObjectType == EObjectType.Creature)
        {
            if (templateID != 0 && Managers.Data.CreatureDic.TryGetValue(templateID , out Data.CreatureData data) == false)
            {
                Debug.LogError($"ObjectManager Spawn Creature Failed! TryGetValue TemplateID : {templateID}");
                return null;
            }

            Creature creature = go.GetComponent<Creature>();
            switch(creature.CreatureType)
            {
                case ECreatureType.Hero:
                    obj.transform.parent = HeroRoot;
                    Hero hero = creature as Hero; // 다이네믹 캐스트 
                    Heros.Add(hero);
                    break;

                case ECreatureType.Monster:
                    obj.transform.parent = MonsterRoot;
                    Monster monster = creature as Monster;
                    Monsters.Add(monster);
                    break;
            }

            creature.SetInfo(templateID);
        }
        else if(obj.ObjectType == EObjectType.Projectile)
        {
            // TODO : Projectile 생기면 넣을 곳 
        }    
        else if(obj.ObjectType == EObjectType.Env)
        {
            if (templateID != 0 && Managers.Data.EnvDic.TryGetValue(templateID , out EnvData data) == false)
            {
                Debug.LogError($"ObjectManager Spawn Env Failed! TryGetValue TemplateID : {templateID}");
                return null;
            }

            obj.transform.parent = EnvRoot;

            Env env = go.GetComponent<Env>();
            Envs.Add(env);

            env.SetInfo(templateID);
        }

        return obj as T;

    }

    public void Despawn<T>(T obj) where T : BaseObject
    {
        EObjectType objectType = obj.ObjectType;
        if (obj.ObjectType == EObjectType.Creature)
        {
            Creature creature = obj.GetComponent<Creature>();
            switch (creature.CreatureType)  
            {
                case ECreatureType.Hero:
                    Hero hero = creature as Hero;
                    Heros.Remove(hero);
                    break;
                case ECreatureType.Monster:
                    Monster monster = creature as Monster;
                    Monsters.Remove(monster);
                    break;
            }
        }
        else if (obj.ObjectType == EObjectType.Projectile)
        {
            // TODO : Projectile Despawn
        }
        else if(obj.ObjectType == EObjectType.Env)
        {
            // TODO : 환경 요소 제거
        }

        Managers.Resource.Destory(obj.gameObject);
    }
}
