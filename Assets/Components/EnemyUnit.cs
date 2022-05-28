using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(EnemyController))]
[RequireComponent(typeof(CircleCollider2D))]
public class EnemyUnit : UnitBase, IKnockbackReceiver
{
    [SerializeField] private Color color;
    [SerializeField] private Animator enemyAnimator;
    [SerializeField] private float movementSpeedAnimationMultiplier;
    [SerializeField] private float movementThreshold;

    private SpriteRenderer _spriteRenderer;
    private CircleCollider2D _collider;

    public Color Color
    {
        get { return color; }
        set { color = value; }
    }

    public Animator EnemyAnimator
    {
        get
        {
            if (enemyAnimator == null)
            {
                enemyAnimator = GetComponent<Animator>();
            }

            return enemyAnimator;
        }
    }

    public CircleCollider2D OwnCollider
    {
        get
        {
            if (_collider == null)
            {
                _collider = GetComponent<CircleCollider2D>();
            }

            return _collider;
        }
    }
    
    public bool Ready { get; private set; }
    
    protected override void Die()
    {
        Ready = false;
        OwnCollider.enabled = false;
        EnemyAnimator.SetBool("Dead", true);
    }

    protected override void Awake()
    {
        base.Awake();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _spriteRenderer.color = color;
        OwnCollider.enabled = true;
    }

    public void TakeKnockback(Vector2 knockbackVector)
    {
        Mover.Physicsbody.AddForce(knockbackVector, ForceMode2D.Impulse);
    }

    public void OnSpawnComplete()
    {
        Ready = true;
        OwnCollider.enabled = true;
    }

    public void OnDeathComplete()
    {
        gameObject.SetActive(false);
        Health.IncreaseHealth(Health.MaxHealth);
    }

    private void Update()
    {
        EnemyAnimator.SetFloat("MovementSpeed", Mover.Physicsbody.velocity.magnitude * movementSpeedAnimationMultiplier);
        EnemyAnimator.SetBool("IsMoving", Mover.Physicsbody.velocity.magnitude > movementThreshold);
        
        if (Mover.Physicsbody.velocity.x < 0)
        {
            _spriteRenderer.flipX = true;
        }
        else if (Mover.Physicsbody.velocity.x > 0)
        {
            _spriteRenderer.flipX = false;
        }
    }
}
