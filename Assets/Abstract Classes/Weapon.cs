using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Timer))]
public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float cooldownTime;
    [SerializeField] private float knockbackStrength;
    [SerializeField] private Timer cooldownTimer;

    public int Damage
    {
        get { return damage; }
    }

    public float KnockbackStrength
    {
        get { return knockbackStrength; }
    }

    public Timer CooldownTimer
    {
        get
        {
            if (cooldownTimer == null)
            {
                cooldownTimer = GetComponent<Timer>();
            }

            return cooldownTimer;
        }
    }
    
    public bool CanUse
    {
        get { return CooldownTimer.IsCompleted && !GameManager.GameIsPaused; }
    }

    public abstract void Use();

    protected void ResetTimer()
    {
        CooldownTimer.SetTime(cooldownTime);
        CooldownTimer.StartTimer();
    }

    protected virtual void Awake()
    {
        cooldownTimer = GetComponent<Timer>();
        ResetTimer();
    }
}
