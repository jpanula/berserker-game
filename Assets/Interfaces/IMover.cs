using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interfaces
{

    public interface IMover
    {
        float Acceleration { get; }
        float MaxSpeed { get; }
        Rigidbody2D Physicsbody { get; }

        void StartMove(Vector2 movementVector);
        void StopMove();
    }
}
