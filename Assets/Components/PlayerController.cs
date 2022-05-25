using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(PlayerUnit))]
public class PlayerController : MonoBehaviour
{
    private IMover _mover;
    private PlayerUnit _playerUnit;
    
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

    public PlayerUnit PlayerUnit
    {
        get
        {
            if (_playerUnit == null)
            {
                _playerUnit = GetComponent<PlayerUnit>();
            }

            return _playerUnit;
        }
    }
    
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PlayerUnit.Attack();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Mover.StartMove(context.ReadValue<Vector2>());
        }

        if (context.canceled)
        {
            Mover.StopMove();
        }
    }
    
}