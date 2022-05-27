using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(EnemyController))]
public class EnemyUnit : UnitBase, IKnockbackReceiver
{
    protected override void Die()
    {
        //TODO return to pool
    }

    public void TakeKnockback(Vector2 knockbackVector)
    {
        Mover.Physicsbody.AddForce(knockbackVector, ForceMode2D.Impulse);
    }
}
