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
        gameOverText.SetText("Game Over!\n{0} enemies killed.", GameManager.TotalEnemiesKilled);
    }
}
