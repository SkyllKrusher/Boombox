using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    #region ----------------------------------------- Init Singleton ----------------------------------------
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion -----------------------------------------------------------------------------------------------
    [SerializeField] private bool logsEnabled;
    private static string GameSceneName = "GameScene";
    private static string MainMenuSceneName = "MainMenuScene";
    private LevelController levelController;
    // private EnemyController enemyController;
    private GameUIView gameUIView;
    private Player player;
    private Grid grid;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        Debug.unityLogger.logEnabled = logsEnabled;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if (scene.name == GameSceneName)
        {
            InitGameScene();
        }
    }

    private void InitGameScene()
    {
        InitGrid();
        InitPlayer();
        InitLevelController();
        levelController.RandomLevel();
        // InitEnemyController();
        InitGameUIView();
    }

    private void InitGrid()
    {
        grid = FindObjectOfType<Grid>();
        grid.Init();
    }

    private void InitPlayer()
    {
        player = FindObjectOfType<Player>();
        ResetPlayer();
    }

    private void InitLevelController()
    {
        levelController = FindObjectOfType<LevelController>();
    }

    // private void InitEnemyController()
    // {
    //     enemyController = FindObjectOfType<EnemyController>();
    //     enemyController.Init();
    // }

    private void InitGameUIView()
    {
        gameUIView = FindObjectOfType<GameUIView>();
    }

    public void Death()
    {
        Debug.Log("ded :(");
        player.DeathSequence();
    }

    public void Lose()
    {
        gameUIView.LoseScreen();
    }

    public void ResetPlayer()
    {
        player.enabled = true;
        player.Init();
    }

    public void LoadGrid(int seed)
    {
        grid.LoadGrid(seed);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(GameSceneName);
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene(MainMenuSceneName);
    }
}
