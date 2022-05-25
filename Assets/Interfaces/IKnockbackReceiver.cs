using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interfaces
{
    public interface IKnockbackReceiver
    {
        public void TakeKnockback(Vector2 knockbackVector);
    }
}

