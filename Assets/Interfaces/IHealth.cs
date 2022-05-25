using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    int CurrentHealth { get; }
    int MaxHealth { get; }
    bool IsInvulnerable { get; set; }
    void IncreaseHealth(int amount);
    
    /// <summary>
    /// Decreases health.
    /// </summary>
    /// <param name="amount">Amount of health to decrease.</param>
    /// <returns>True if health decreases to 0 or below, false otherwise.</returns>
    bool DecreaseHealth(int amount);
}