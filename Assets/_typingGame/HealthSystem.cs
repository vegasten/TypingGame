using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event Action OnDeath;

    [SerializeField] int _startHealth = 5;
    public int Health { get; private set; }

    private void Start()
    {
        Health = _startHealth;
    }

    public void TakeDamage(int damage)
    {
        Health -= Mathf.Max(damage, 0);

        if (Health == 0)
        {
            die();
        }
    }

    public void ResetHealth()
    {
        Health = _startHealth;
    }

    private void die()
    {
        OnDeath?.Invoke();
    }
}
