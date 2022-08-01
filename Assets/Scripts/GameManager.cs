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

    public void Lose()
    {
        Debug.Log("Lost :(");
    }
}
