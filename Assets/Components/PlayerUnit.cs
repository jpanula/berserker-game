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

    private List<Vector3> _knockbackVectors = new List<Vector3>();

    public float DamageTakenCooldown
    {
        get { return damageTakenCooldown; }
    }

    public bool IsDead { get; private set; }

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
        InvulnerabilityTimer.SetTime(DamageTakenCooldown);
        InvulnerabilityTimer.StartTimer();
        if (!Health.IsInvulnerable)
        {
            PlayerAnimator.SetTrigger("Damage");
        }
        return result;
    }

    protected override void Die()
    {
        playerDeathEvent.Invoke();
        PlayerAnimator.SetBool("Dead", true);
        IsDead = true;
    }

    public void IsActuallyDead()
    {
        playerAnimator.enabled = false;
        if (GameManager.PlayerIsBerserk)
        {
            GameManager.Instance.GameOverEvent.Invoke();
        }
        else
        {
            GameManager.PlayerIsBerserk = true;
            GameManager.Instance.GoingBerserkEvent.Invoke();
        }
    }
    
    public void TakeKnockback(Vector2 knockbackVector)
    {
        if (!Health.IsInvulnerable)
        {
            _knockbackVectors.Add(knockbackVector);
        }
    }

    private void OnGameOver()
    {
        Health.IsInvulnerable = false;
        Health.DecreaseHealth(Health.MaxHealth);
    }

    private void ExecuteKnockback()
    {
        foreach (var vector in _knockbackVectors)
        {
            Mover.Physicsbody.AddForce(vector, ForceMode2D.Impulse);
        }
        _knockbackVectors.Clear();
    }
    
    private void Start()
    {
        GameManager.Instance.GameOverEvent.AddListener(OnGameOver);
        if(blinkCooldown != 0)
        {
            if (blinkTimer)
            {
                blinkTimer.SetTime(blinkCooldown + Random.value * blinkVariance);
                blinkTimer.StartTimer();
            }
        }
    }

    private void OnEnable()
    {
        playerAnimator.enabled = true;
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

        if (Mover.Physicsbody.velocity.x < 0  && !IsDead)
        {
            PlayerSpriteRenderer.flipX = true;
        }
        else if (Mover.Physicsbody.velocity.x > 0 && !IsDead)
        {
            PlayerSpriteRenderer.flipX = false;
        }
        
        if (blinkTimer && blinkCooldown != 0)
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

    private void FixedUpdate()
    {
        ExecuteKnockback();
    }
}
