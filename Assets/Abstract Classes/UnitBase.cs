using UnityEngine;

public abstract class UnitBase : MonoBehaviour, IDamageReceiver
{
    private IMover _mover;
    private IHealth _health;

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


    public bool TakeDamage(int amount)
    {
        bool died = Health.DecreaseHealth(amount);
        if (died)
        {
            Die();
        }

        return died;
    }

    protected abstract void Die();
}