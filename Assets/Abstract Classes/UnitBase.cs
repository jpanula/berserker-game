using System;
using UnityEngine;

public abstract class UnitBase : MonoBehaviour, IDamageReceiver
{

    private IMover _mover;
    private IHealth _health;
    private Weapon[] _weapons;

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

    public IHealth Health
    {
        get
        {
            if (_health == null)
            {
                _health = GetComponent<IHealth>();
            }

            return _health;
        }
    }


    public virtual bool TakeDamage(int amount)
    {
        bool died = Health.DecreaseHealth(amount);
        if (died)
        {
            Die();
        }

        return died;
    }

    public virtual void Attack()
    {
        foreach (var weapon in _weapons)
        {
            weapon.Use();
        }
    }
    
    protected abstract void Die();

    protected virtual void Awake()
    {
        _weapons = GetComponentsInChildren<Weapon>(includeInactive: true);
    }
}