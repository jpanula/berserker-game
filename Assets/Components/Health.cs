using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IHealth
{
    [SerializeField] private int maxHealth;
    private int _currentHealth;

    public int CurrentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = Mathf.Clamp(value, 0, MaxHealth); }
    }

    public int MaxHealth
    {
        get { return maxHealth; }
    }
    public bool IsInvulnerable { get; set; }
    public void IncreaseHealth(int amount)
    {
        CurrentHealth += amount;
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
    }

    public bool DecreaseHealth(int amount)
    {
        if (!IsInvulnerable)
        {
            CurrentHealth -= amount;
        }

        return (CurrentHealth <= 0);
    }
}