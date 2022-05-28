using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CircleCollider2D))]
public class PlayerWeapon : Weapon
{
    
    [SerializeField] private float movementRadius;
    [SerializeField] private float aimingSpeed;
    [SerializeField] private CircleCollider2D attackCollider;
    
    private Camera _mainCamera;
    private Vector3 _mousePosition;
    private Animator _animator;

    public CircleCollider2D AttackCollider
    {
        get
        {
            if (attackCollider == null)
            {
                attackCollider = GetComponent<CircleCollider2D>();
            }

            return attackCollider;
        }
    }

    public Animator Animator
    {
        get
        {
            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
            }

            return _animator;
        }
    }
    
    public override void Use()
    {
        if (CanUse)
        {
            Animator.SetTrigger("Attack");
            ResetTimer();
            List<Collider2D> hitColliders = new List<Collider2D>();
            AttackCollider.GetContacts(hitColliders);
            foreach (var collider in hitColliders)
            {
                if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    
                    if (collider.GetComponent<IDamageReceiver>() != null)
                    {
                        collider.GetComponent<IDamageReceiver>().TakeDamage(Damage);
                    }

                    if (collider.GetComponent<IKnockbackReceiver>() != null)
                    {
                        collider.GetComponent<IKnockbackReceiver>()
                            .TakeKnockback(
                                (collider.transform.position - transform.parent.position) * KnockbackStrength);
                    }
                }
            }
        }
    }
    
//TODO separate pointer maybe
    
    private void Start()
    {
        _mainCamera = Camera.main;
        transform.localPosition = new Vector3(0, movementRadius, 0);
    }

    public void OnPointer(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && !GameManager.GameIsPaused)
        {
            _mousePosition = _mainCamera.ScreenToWorldPoint(callbackContext.ReadValue<Vector2>());
        }
    }

    private void Update()
    {
        _mousePosition.z = 0;
        var newPosition = _mousePosition - transform.parent.position;
        newPosition = newPosition.normalized * movementRadius;

        transform.localPosition = newPosition;
    }
}
