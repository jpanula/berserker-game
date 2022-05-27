using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Timer))]
[RequireComponent(typeof(Animator))]
public class PlayerUnit : UnitBase, IKnockbackReceiver
{
    public UnityEvent playerDeathEvent;

    [SerializeField] private float damageTakenCooldown;
    [SerializeField] private float blinkCooldown;
    [SerializeField] private float blinkVariance;
    [SerializeField] private float movementThreshold;
    [SerializeField] private float movementSpeedAnimationMultiplier;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Timer invulnerabilityTimer;
    [SerializeField] private Timer blinkTimer;

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

    public SpriteRenderer PlayerSpriteRenderer
    {
        get
        {
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }

            return spriteRenderer;
        }
    }
    
    public Animator PlayerAnimator
    {
        get
        {
            if (playerAnimator == null)
            {
                playerAnimator = GetComponent<Animator>();
            }

            return playerAnimator;
        }
    }
    
    public override bool TakeDamage(int amount)
    {
        bool result = base.TakeDamage(amount);
        InvulnerabilityTimer.SetTime(damageTakenCooldown);
        InvulnerabilityTimer.StartTimer();
        PlayerAnimator.SetTrigger("Damage");
        return result;
    }

    protected override void Die()
    {
        playerDeathEvent.Invoke();
        PlayerAnimator.SetBool("Dead", true);
    }

    public void TakeKnockback(Vector2 knockbackVector)
    {
        Mover.Physicsbody.AddForce(knockbackVector, ForceMode2D.Impulse);
    }

    private void Start()
    {
        if (blinkTimer)
        {
            blinkTimer.SetTime(blinkCooldown + Random.value * blinkVariance);
            blinkTimer.StartTimer();
        }
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
        
        PlayerAnimator.SetFloat("MovementSpeed", Mover.Physicsbody.velocity.magnitude * movementSpeedAnimationMultiplier);
        PlayerAnimator.SetBool("IsMoving", Mover.Physicsbody.velocity.magnitude > movementThreshold);

        if (Mover.Physicsbody.velocity.x < 0)
        {
            PlayerSpriteRenderer.flipX = true;
        }
        else if (Mover.Physicsbody.velocity.x > 0)
        {
            PlayerSpriteRenderer.flipX = false;
        }
        
        if (blinkTimer)
        {
            if (!PlayerAnimator.GetBool("IsMoving"))
            {
                if (blinkTimer.IsCompleted)
                {
                    PlayerAnimator.SetTrigger("Blink");
                    blinkTimer.SetTime(blinkCooldown + Random.value * blinkVariance);
                    blinkTimer.StartTimer();
                }
            }
            
        }
    }
}
