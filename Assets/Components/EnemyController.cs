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
        _player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        var ownTransform = transform;
        var currentPosition = ownTransform.position;
        var playerPosition = _player.position;
        var movementVector = playerPosition - currentPosition;
        RaycastHit2D hit;
        hit = Physics2D.CircleCast(currentPosition,
            Collider.radius * Mathf.Max(ownTransform.localScale.x, ownTransform.localScale.y), movementVector, movementVector.magnitude, LayerMask.GetMask("Environment"));

        if (hit)
        {
            var path = PathGridManager.Instance.FindPath(currentPosition, playerPosition);
            if (path.Count >= 1)
            {

                movementVector = path[0].Position - currentPosition;
            }
        }

        Mover.StartMove(Vector3.Normalize(movementVector));
    }
}
