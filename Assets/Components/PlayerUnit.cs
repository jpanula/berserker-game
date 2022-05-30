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
    [SerializeField] private TrailRenderer[] trails;
    [SerializeField] private GameObject trailParent;

    private List<Vector3> _knockbackVectors = new List<Vector3>();
    private bool _goingBerserk;
    private bool _trailsFlipped;

    public bool GoingBerserk
    {
        get { return _goingBerserk; }
        set { _goingBerserk = value; }
    }
    
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
        if (!Health.IsInvulnerable && !GameManager.PlayerIsBerserk)
        {
            PlayerAnimator.SetTrigger("Damage");
        }
        if (!Health.IsInvulnerable)
        {
            InvulnerabilityTimer.SetTime(DamageTakenCooldown);
            InvulnerabilityTimer.StartTimer();
            Health.IsInvulnerable = true;
        }
        
        if (InvulnerabilityTimer.IsRunning)
        {
            Health.IsInvulnerable = true;
        }
        return result;
    }

    protected override void Die()
    {
        playerDeathEvent.Invoke();
        PlayerAnimator.SetBool("Dead", true);
        IsDead = true;
        GameManager.EnemiesActive = false;
    }

    public void Kill()
    {
        Health.IsInvulnerable = false;
        TakeDamage(Health.MaxHealth);
    }
    
    public void IsActuallyDead()
    {
        PlayerAnimator.SetBool("Dead", false);
        playerAnimator.enabled = false;
        if (GameManager.PlayerIsBerserk)
        {
            GameManager.Instance.GameOverEvent.Invoke();
        }
        else
        {
            GameManager.PlayerIsBerserk = true;
            GameManager.Instance.GoingBerserkEvent.Invoke();
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void OnResurrection()
    {
        Health.IncreaseHealth(Health.MaxHealth);
        IsDead = false;
        Health.IsInvulnerable = false;
        _goingBerserk = false;
    }

    private void OnGoingBerserk()
    {
        GetComponent<Mover>().enabled = false;
        Health.IsInvulnerable = true;
        _goingBerserk = true;
    }

    public void OnGoingBerserkFinished()
    {
        ResetPlayer();
        GetComponent<Mover>().enabled = true;
        Health.IsInvulnerable = false;
        _goingBerserk = false;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        GameManager.EnemiesActive = true;
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
        GameManager.Instance.ResurrectionEvent.AddListener(OnResurrection);
        GameManager.Instance.GoingBerserkEvent.AddListener(OnGoingBerserk);
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
        InvulnerabilityTimer.Stop();
        InvulnerabilityTimer.SetTime(DamageTakenCooldown);
        InvulnerabilityTimer.StartTimer();
        ResetPlayer();
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

        foreach (var trail in trails)
        {
            if (trail != null)
            {
                trail.emitting = PlayerAnimator.GetBool("IsMoving");
            }
        }

        if (trailParent != null)
        {
            if (_trailsFlipped)
            {
                trailParent.transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                trailParent.transform.localScale = new Vector3(1, 1, 1);
            }
        }
        
        if (Mover.Physicsbody.velocity.x < 0  && !IsDead && !_goingBerserk)
        {
            PlayerSpriteRenderer.flipX = true;
            _trailsFlipped = true;
        }
        else if (Mover.Physicsbody.velocity.x > 0 && !IsDead && !_goingBerserk)
        {
            PlayerSpriteRenderer.flipX = false;
            _trailsFlipped = false;
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

    private void ResetPlayer()
    {
        IsDead = false;
        Health.IncreaseHealth(Health.MaxHealth);
        Health.IsInvulnerable = false;
        if(blinkCooldown != 0)
        {
            if (blinkTimer)
            {
                blinkTimer.SetTime(blinkCooldown + Random.value * blinkVariance);
                blinkTimer.StartTimer();
            }
        }
        InvulnerabilityTimer.Stop();
        InvulnerabilityTimer.SetTime(DamageTakenCooldown);
        InvulnerabilityTimer.StartTimer();
        PlayerAnimator.SetFloat("MovementSpeed", 0);
        PlayerAnimator.SetBool("IsMoving", false);
        PlayerAnimator.SetBool("Dead", false);
        _knockbackVectors.Clear();
        Mover.StopMove();
        GetComponent<Mover>().enabled = true;
    }
}
