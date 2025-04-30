using UnityEngine;

/// <summary>
/// 전역으로 관리할 Managers
/// </summary>
public class Managers : MonoBehaviour
{
    public static Managers Instance { get { Init(); return s_instance; } }
    static Managers s_instance;

    #region Content
    private GameManager _game = new GameManager();
    private ObjectManager _object = new ObjectManager();

    public static GameManager Game { get { return Instance?._game; } }
    public static ObjectManager Object { get { return Instance?._object; } }
    #endregion

    #region Core
    public static ResourceManager Resource => Instance?._resource;
    public static DataManager Data => Instance?._data;
    public static PoolManager Pool => Instance?._pool;
    public static SoundManager Sound => Instance?._sound;
    public static SceneManagerEX Scene => Instance?._scene;
    public static UI_Manager UI => Instance?._ui;

    DataManager _data = new DataManager();
    ResourceManager _resource = new ResourceManager(); 
    PoolManager _pool = new PoolManager();
    SoundManager _sound = new SoundManager();
    SceneManagerEX _scene = new SceneManagerEX();
    UI_Manager _ui = new UI_Manager();
    #endregion

    /// <summary>
    /// Managers 생성 로직 
    /// </summary>
    public static void Init()
    {
        if (s_instance == null)
        {
            GameObject go =  GameObject.Find("@Managers");

            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);

            s_instance = go.GetComponent<Managers>();
        }

    }
}
