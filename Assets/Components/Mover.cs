using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Components
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Mover : MonoBehaviour, IMover
    {
        [SerializeField] private float acceleration = 100;
        [SerializeField] private float maxSpeed = 5.5f;
        private Vector2 _movementVector;
        private Rigidbody2D _physicsbody;

        public float Acceleration
        {
            get { return acceleration; }
            set { acceleration = value; }
        }

        public float MaxSpeed
        {
            get { return maxSpeed; }
        }

        public Vector2 MovementVector
        {
            get { return _movementVector; }
            set { _movementVector = value; }
        }

        public Rigidbody2D Physicsbody
        {
            get
            {
                if (_physicsbody == null)
                {
                    _physicsbody = GetComponent<Rigidbody2D>();
                }

                return _physicsbody;
            }
        }

        public void StartMove(Vector2 movementVector)
        {
            MovementVector = movementVector;
        }

        public void StopMove()
        {
            MovementVector = Vector2.zero;
        }

        private void Reset()
        {
            Physicsbody.freezeRotation = true;
            Physicsbody.drag = 10;
        }

        private void FixedUpdate()
        {
            Physicsbody.AddForce(MovementVector * acceleration);
            
            if (Physicsbody.velocity.magnitude > MaxSpeed)
            {
                Physicsbody.velocity = Vector2.ClampMagnitude(Physicsbody.velocity, MaxSpeed);
            }
        }
    }
}

