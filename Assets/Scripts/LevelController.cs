using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public int levelNo { get; private set; }

    private void Start()
    {
        // Init();
        // StartCoroutine(NextLevelEveryXSeconds(10f));
    }

    private IEnumerator NextLevelEveryXSeconds(float x)
    {
        while (true)
        {
            yield return new WaitForSeconds(x);
            NextLevel();
        }
    }

    public void NextLevel()
    {
        LoadLevel(levelNo + 1);
    }

    private void LoadLevel(int level)
    {
        levelNo = level;
        GameManager.Instance.LoadGrid(level);
        GameManager.Instance.ResetPlayer();
    }

    public void RandomLevel()
    {
        int randomSeed = (int)System.DateTime.Now.Ticks;
        LoadLevel(randomSeed);
    }


}
