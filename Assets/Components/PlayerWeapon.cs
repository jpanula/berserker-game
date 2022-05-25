using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : Weapon
{
    [SerializeField] private float movementRadius;
    [SerializeField] private float aimingSpeed;
    
    private Camera _mainCamera;
    private PlayerInput _playerInput;
    
    public override void Use()
    {
        //TODO Implement player attack
    }
//TODO separate pointer
    
    private void Start()
    {
        _mainCamera = Camera.main;
        _playerInput = GetComponentInParent<PlayerInput>();
        transform.localPosition = new Vector3(0, movementRadius, 0);
    }

    public void OnPointer(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            var cursorLocation = _mainCamera.ScreenToWorldPoint(callbackContext.ReadValue<Vector2>());
            cursorLocation.z = 0;
            var newPosition = cursorLocation - transform.parent.position;
            newPosition = newPosition.normalized * movementRadius;

            transform.localPosition = newPosition;
        }
    }
}
