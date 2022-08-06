using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIView : MonoBehaviour
{
    [SerializeField] private Button homeButton;
    [SerializeField] private GameObject lossPanel;

    private void Awake()
    {
        homeButton.onClick.AddListener(() => OnHomeButtnClick());
    }

    private void OnHomeButtnClick()
    {
        GameManager.Instance.LoadMenuScene();
    }

    public void LoseScreen()
    {
        lossPanel.SetActive(true);
    }
}
