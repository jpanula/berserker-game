using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    private bool _berserking;

    public int EnemiesKilled { get; private set; }

    private void Start()
    {
        GameManager.Instance.GoingBerserkEvent.AddListener(ActivateBerserker);
        GameManager.Instance.ResurrectionEvent.AddListener(DeactivateBerserker);
        GameManager.Instance.EnemyKilledEvent.AddListener(OnEnemyKill);
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
    }

    private void DeactivateBerserker()
    {
        berserker.SetActive(false);
        player.transform.position = berserker.transform.position;
        player.SetActive(true);
        _berserking = false;
    }

    private void OnEnemyKill()
    {
        EnemiesKilled++;
    }

    private void Update()
    {
        if (EnemiesKilled >= minimumKillsToResurrect)
        {
            GameManager.Instance.ResurrectionEvent.Invoke();
        }
        
        killsNeededText.SetText("Kill {0} enemies\nto resurrect", minimumKillsToResurrect - EnemiesKilled);
        resurrectTimeBar.value = berserkTimer.CurrentTime / timeToResurrect;

        if (berserkTimer.IsCompleted && _berserking)
        {
            GameManager.Instance.GameOverEvent.Invoke();
        }
    }
}
