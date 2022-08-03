using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // private static string GameSceneName = "GameScene";
    // private static string MainMenuSceneName = "MainMenuScene";
    private LevelController levelController;
    private Player player;
    private Grid grid;

    private void Start()
    {
        InitGrid();
        InitPlayer();
        InitLevelController();
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
        levelController.Init();
    }

    public void Death()
    {
        Debug.Log("ded :(");
        // player.Death();
    }

    public void ResetPlayer()
    {
        player.enabled = true;
        player.Init();
    }

    public void LoadGrid(int seed = 0)
    {
        grid.LoadGrid(seed);
    }
}
