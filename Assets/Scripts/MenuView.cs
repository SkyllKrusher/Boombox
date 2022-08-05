using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuView : MonoBehaviour
{
    [SerializeField] private Button playButton;

    void Awake()
    {
        playButton.onClick.AddListener(() => OnClickPlay());
    }

    private void OnClickPlay()
    {
        GameManager.Instance.LoadGameScene();
    }
}
