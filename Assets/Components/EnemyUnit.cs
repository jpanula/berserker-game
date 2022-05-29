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
    [SerializeField] private int damage;
    [SerializeField] private float knockbackStrength;

    private SpriteRenderer _spriteRenderer;
    private CircleCollider2D _collider;
    private List<Vector3> _knockbackVectors = new List<Vector3>();

    public int Damage
    {
        get { return damage; }
    }

    public float KnockbackStrength
    {
        get { return knockbackStrength; }
    }
    
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
        GameManager.Instance.EnemyKilledEvent.Invoke();
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
        if (!Health.IsInvulnerable)
        {
            _knockbackVectors.Add(knockbackVector);
        }
    }

    private void ExecuteKnockback()
    {
        foreach (var vector in _knockbackVectors)
        {
            Mover.Physicsbody.AddForce(vector, ForceMode2D.Impulse);
        }
        _knockbackVectors.Clear();
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
    
    private void FixedUpdate()
    {
        ExecuteKnockback();
    }
}
