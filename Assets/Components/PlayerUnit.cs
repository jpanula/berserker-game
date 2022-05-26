using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Timer))]
public class PlayerUnit : UnitBase, IKnockbackReceiver
{
    public UnityEvent playerDeathEvent;

    [SerializeField] private float damageTakenCooldown;
    [SerializeField] private Timer invulnerabilityTimer;
    
    public float DamageTakenCooldown
    {
        get { return damageTakenCooldown; }
    }

    public Timer InvulnerabilityTimer
    {
        get
        {
            if (invulnerabilityTimer == null)
            {
                invulnerabilityTimer = GetComponent<Timer>();
            }

            return invulnerabilityTimer;
        }
    }

    public override bool TakeDamage(int amount)
    {
        bool result = base.TakeDamage(amount);
        InvulnerabilityTimer.SetTime(damageTakenCooldown);
        InvulnerabilityTimer.StartTimer();
        return result;
    }

    protected override void Die()
    {
        playerDeathEvent.Invoke();
    }

    public void TakeKnockback(Vector2 knockbackVector)
    {
        Mover.Physicsbody.AddForce(knockbackVector, ForceMode2D.Impulse);
    }

    private void Update()
    {
        if (InvulnerabilityTimer.IsRunning)
        {
            Health.IsInvulnerable = true;
        }
        else if (InvulnerabilityTimer.IsCompleted)
        {
            Health.IsInvulnerable = false;
        }
    }
}
