using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Mover))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private IMover _mover;

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