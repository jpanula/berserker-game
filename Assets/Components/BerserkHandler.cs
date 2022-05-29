using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Timer))]
public class BerserkHandler : MonoBehaviour
{
    [SerializeField] private GameObject berserker;
    [SerializeField] private GameObject player;
    [SerializeField] private int minimumKillsToResurrect;
    [SerializeField] private float timeToResurrect;
    [SerializeField] private float minimumTimeToResurrect;
    [SerializeField] private float subsequentTimeToResurrectMultiplier;
    [SerializeField] private Timer berserkTimer;
    [SerializeField] private TextMeshProUGUI killsNeededText;
    [SerializeField] private Slider resurrectTimeBar;
    [SerializeField] private GameObject resurrectUI;
    [FormerlySerializedAs("berserkSpriteRenderer")] [SerializeField] private SpriteRenderer slashSpriteRenderer;
    [SerializeField] private PlayerUnit berserkerPlayer;

    private bool _berserking;

    public int EnemiesKilled { get; private set; }

    private void Start()
    {
        GameManager.Instance.GoingBerserkEvent.AddListener(ActivateBerserker);
        GameManager.Instance.ResurrectionEvent.AddListener(DeactivateBerserker);
        GameManager.Instance.EnemyKilledEvent.AddListener(OnEnemyKill);
        GameManager.Instance.GameOverEvent.AddListener(OnGameOver);
    }

    private void ActivateBerserker()
    {
        EnemiesKilled = 0;
        player.SetActive(false);
        berserker.transform.position = player.transform.position;
        berserker.SetActive(true);
        timeToResurrect = Mathf.Max(minimumTimeToResurrect, timeToResurrect * subsequentTimeToResurrectMultiplier);
        berserkTimer.SetTime(timeToResurrect);
        berserkTimer.StartTimer();
        _berserking = true;
        GameManager.PlayerIsBerserk = true;
    }

    private void DeactivateBerserker()
    {
        slashSpriteRenderer.sprite = null;
        berserker.SetActive(false);
        player.transform.position = berserker.transform.position;
        player.SetActive(true);
        _berserking = false;
        GameManager.PlayerIsBerserk = false;
    }

    private void OnEnemyKill()
    {
        EnemiesKilled++;
    }

    private void OnGameOver()
    {
        resurrectUI.SetActive(false);
    }

    private void Update()
    {
        if (EnemiesKilled >= minimumKillsToResurrect && _berserking)
        {
            GameManager.Instance.ResurrectionEvent.Invoke();
        }
        
        killsNeededText.SetText("<alpha=#AA>Kill <alpha=#FF><color="+"red"+"><b><size= 72>{0}</size></b></color><alpha=#AA> enemies\nto resurrect", minimumKillsToResurrect - EnemiesKilled);
        resurrectTimeBar.value = berserkTimer.CurrentTime / timeToResurrect;

        if (berserkTimer.IsCompleted && _berserking && !GameManager.GameIsPaused)
        {
            _berserking = false;
            berserkerPlayer.Kill();
        }
    }
}
