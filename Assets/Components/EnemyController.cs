using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(EnemyUnit))]
[RequireComponent(typeof(CircleCollider2D))]
public class EnemyController : MonoBehaviour
{
    private IMover _mover;
    private EnemyUnit _enemyUnit;
    private Transform _player;
    private CircleCollider2D _collider;
    
    public IMover Mover
    {
        get
        {
            if (_mover == null)
            {
                _mover = GetComponent<IMover>();
            }

            return _mover;
        }
    }

    public EnemyUnit EnemyUnit
    {
        get
        {
            if (_enemyUnit == null)
            {
                _enemyUnit = GetComponent<EnemyUnit>();
            }

            return _enemyUnit;
        }
    }

    public CircleCollider2D Collider
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

    private void Start()
    {
        FindPlayer();
        GameManager.Instance.GoingBerserkEvent.AddListener(FindPlayer);
        GameManager.Instance.ResurrectionEvent.AddListener(FindPlayer);
    }

    private void FindPlayer()
    {
        _player = GameObject.FindWithTag("Player").transform;
    }
    
    private void FixedUpdate()
    {
        if (EnemyUnit.Ready)
        {
            var ownTransform = transform;
            var currentPosition = ownTransform.position;
            var playerPosition = _player.position;
            var movementVector = playerPosition - currentPosition;
            RaycastHit2D hit;
            hit = Physics2D.CircleCast(currentPosition,
                Collider.radius * Mathf.Max(ownTransform.localScale.x, ownTransform.localScale.y), movementVector,
                movementVector.magnitude, LayerMask.GetMask("Environment"));

            if (hit)
            {
                var path = PathGridManager.Instance.FindPath(currentPosition, playerPosition);
                if (path.Count >= 1)
                {

                    movementVector = path[0].Position - currentPosition;
                }
            }

            Mover.StartMove(Vector3.Normalize(movementVector));

            Collider2D[] colliders =
                Physics2D.OverlapCircleAll(currentPosition, Collider.radius, LayerMask.GetMask("Player"));
            foreach (var foundCollider in colliders)
            {
                var player = foundCollider.GetComponent<PlayerUnit>();
                if (player)
                {
                    if (!player.IsDead)
                    {
                        player.TakeDamage(EnemyUnit.Damage);
                    }
                    player.TakeKnockback(movementVector.normalized * EnemyUnit.KnockbackStrength);
                }
            }
        }
        else
        {
            Mover.StopMove();
        }
    }
}
