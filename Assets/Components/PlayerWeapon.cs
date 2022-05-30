using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerWeapon : Weapon
{
    
    [SerializeField] private float movementRadius;
    [SerializeField] private float aimingSpeed;
    [SerializeField] private PlayerUnit player;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip fireAudio;
    
    private Camera _mainCamera;
    private Vector3 _mousePosition;
    private Animator _animator;
    private bool _used;

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
        if (CanUse && !player.IsDead)
        {
            Animator.SetTrigger("Attack");
            audioSource.clip = fireAudio;
            audioSource.Play();
            if (GameManager.PlayerIsBerserk)
            {
                playerAnimator.SetTrigger("Attack");
            }
            ResetTimer();
            _used = true;
        }
    }

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

    private void OnTriggerStay2D(Collider2D col)
    {
        var enemy = col.GetComponent<EnemyUnit>();
        if (enemy && _used)
        {
            enemy.TakeKnockback(Vector3.Normalize(transform.right) * KnockbackStrength);
            enemy.TakeDamage(Damage);
            _used = false;
        }
    }
}
