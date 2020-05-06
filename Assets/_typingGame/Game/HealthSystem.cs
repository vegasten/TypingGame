using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event Action OnDeath;

    [SerializeField] private HealthPresenter _healthPresenter = null;
    [SerializeField] private int _startHealth = 5;
    public int Health { get; private set; }

    private void Start()
    {
        Health = _startHealth;
        _healthPresenter.UpdateHealth(Health);
    }

    public void TakeDamage(int damage)
    {
        Health -= Mathf.Max(damage, 0);
        _healthPresenter.UpdateHealth(Health);

        if (Health == 0)
        {
            die();
        }
    }

    public void ResetHealth()
    {
        Health = _startHealth;
        _healthPresenter.UpdateHealth(Health);
    }

    private void die()
    {
        OnDeath?.Invoke();
    }
}
