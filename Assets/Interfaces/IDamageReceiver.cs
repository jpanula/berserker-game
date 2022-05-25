using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageReceiver
{
    public bool TakeDamage(int amount);
}