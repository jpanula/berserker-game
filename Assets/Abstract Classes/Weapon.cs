using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Timer))]
public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float cooldownTime;

    private Timer _cooldownTimer;

    public int Damage
    {
        get { return damage; }
    }
    
    public bool CanUse
    {
        get { return _cooldownTimer.IsCompleted; }
    }

    public abstract void Use();

    protected void ResetTimer()
    {
        _cooldownTimer.SetTime(cooldownTime);
        _cooldownTimer.StartTimer();
    }

    protected virtual void Awake()
    {
        _cooldownTimer = GetComponent<Timer>();
    }
}
