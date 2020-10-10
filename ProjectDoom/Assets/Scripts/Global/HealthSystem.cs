using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HealthSystem
{
    public event EventHandler OnHealthChanged;
    public event EventHandler OnHealthMaxChanged;
    public event EventHandler OnDamaged;
    public event EventHandler OnHealed;
    public event EventHandler OnDead;

    private int health;
    private int healthMax;

    public HealthSystem(int healthMax)
    {
        this.healthMax = healthMax;
        health = healthMax;
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetHealthMax()
    {
        return healthMax;
    }

    public float GetHealthNormalized()
    {
        return (float)health / healthMax;
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;

        if(health < 0)
        {
            health = 0;
        }

        OnHealthChanged?.Invoke(this, EventArgs.Empty);
        OnDamaged?.Invoke(this, EventArgs.Empty);

        if(health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    public bool IsDead()
    {
        return health <= 0;
    }

    public void Heal(int healAmount)
    {
        health += healAmount;

        if(health > healthMax)
        {
            health = healthMax;
        }

        OnHealthChanged?.Invoke(this, EventArgs.Empty);
        OnHealed?.Invoke(this, EventArgs.Empty);
    }

    public void HealComplete()
    {
        health = healthMax;
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
        OnHealed?.Invoke(this, EventArgs.Empty);
    }

    public void SetHealthMax(int healthMax, bool fullHealth)
    {
        this.healthMax = healthMax;

        if (fullHealth)
        {
            health = healthMax;
        }

        OnHealthMaxChanged?.Invoke(this, EventArgs.Empty);
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }
}
