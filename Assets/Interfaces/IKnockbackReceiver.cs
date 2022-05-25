using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKnockbackReceiver
{
    public void TakeKnockback(Vector2 knockbackVector);
}