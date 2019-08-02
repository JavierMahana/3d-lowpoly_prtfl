using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Lean.Pool;

public class HealthBarManager : MonoBehaviour
{
    [AssetsOnly]
    [Required]
    [SerializeField]
    private HealthBar healthBarPrefab;

    private Dictionary<Health, HealthBar> ActiveHealthBars = new Dictionary<Health, HealthBar>();

    private void Awake()
    {
        Health.RequestHealthbar += AddHealthBar;
        Health.RemoveHealthBar += RemoveHealthBar;
    }

    private void AddHealthBar(Health health)
    {
        if (ActiveHealthBars.ContainsKey(health))
        {
            return;
        }

        HealthBar hb = LeanPool.Spawn(healthBarPrefab, Vector3.zero, Quaternion.identity, this.transform);
        hb.transform.localScale = Vector3.one;

        hb.SetHealth(health);
        ActiveHealthBars.Add(health, hb);
    }
    private void RemoveHealthBar(Health health)
    {
        if (ActiveHealthBars.TryGetValue(health, out HealthBar hb))
        {
            ActiveHealthBars.Remove(health);
            LeanPool.Despawn(hb);
        }


    }
}
