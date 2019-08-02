using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using Lean.Pool;

public class Health : MonoBehaviour
{
    public static Action<Health> RequestHealthbar = delegate { };
    public static Action<Health> RemoveHealthBar = delegate { };


    [SerializeField]
    private DefensiveStats defensiveStats;

    private bool isTheHealthBarActive = false;

    public Entity HealthsEntity;

    [ReadOnly]
    public int CurrentHealth;
    [SerializeField]
    private int maxHealth;
    [HideInInspector]
    public Action<float, Health> OnHealthChanged = delegate { };


    private void OnEnable()
    {
        CurrentHealth = maxHealth;
    }
    private void OnDisable()
    {
        RemoveHealthBar(this);
        isTheHealthBarActive = false;
    }

    /// <param name="amount">positive values heal the unit. negative values damge the unit</param>
    public virtual void ModifyHealth(int amount)
    {
        if (amount == 0)
            return;

        else if (amount < 0)
            defensiveStats.TakeDamage(ref CurrentHealth, -amount);

        else
            CurrentHealth += amount;

        CurrentHealth = Mathf.Min(CurrentHealth, maxHealth);

        if (CurrentHealth <= 0)
        {
            LeanPool.Despawn(this);
            return;
        }
        TryToDisplayOrHideHealthBar(ref isTheHealthBarActive);

        float currentPrcHealth = (float)CurrentHealth / (float)maxHealth;
        OnHealthChanged(currentPrcHealth, this);
    }



    public void TryToDisplayOrHideHealthBar(ref bool isTheHealthBarActive)
    {
        if (CurrentHealth < maxHealth && isTheHealthBarActive == false)
        {
            RequestHealthbar(this);
            isTheHealthBarActive = true;
        }
        else if (CurrentHealth >= maxHealth && isTheHealthBarActive)
        {
            RemoveHealthBar(this);
            isTheHealthBarActive = false;
        }
    }
}
