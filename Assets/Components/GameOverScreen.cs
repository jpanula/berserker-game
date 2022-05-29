using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TextMeshProUGUI gameOverText;
    
    private void Start()
    {
        GameManager.Instance.GameOverEvent.AddListener(DisplayGameOverScreen);
    }

    private void DisplayGameOverScreen()
    {
        gameOverScreen.SetActive(true);
        gameOverText.SetText("Game Over!\n<color=white><size= 96>{0}</size></color> enemies killed.", GameManager.TotalEnemiesKilled);
    }
}
