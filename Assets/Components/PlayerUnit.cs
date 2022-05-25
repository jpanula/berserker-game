using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(PlayerController))]
public class PlayerUnit : UnitBase, IKnockbackReceiver
{

    protected override void Die()
    {
        //TODO Implement player death
    }

    public void TakeKnockback(Vector2 knockbackVector)
    {
        Mover.Physicsbody.AddForce(knockbackVector);
    }

}
