using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(PlayerController))]
public class PlayerUnit : UnitBase, IKnockbackReceiver
{
    public UnityEvent playerDeathEvent;

    protected override void Die()
    {
        playerDeathEvent.Invoke();
    }

    public void TakeKnockback(Vector2 knockbackVector)
    {
        Mover.Physicsbody.AddForce(knockbackVector);
    }

}
