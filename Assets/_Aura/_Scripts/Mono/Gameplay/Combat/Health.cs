using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Reusable Health Component
/// </summary>
public class Health : NetworkBehaviour
{
    //synced health across all clients
    public NetworkVariable<int> CurrentHealth = new NetworkVariable<int>();

    //max health. does not change so no need to network sync
    [field: SerializeField] public int MaxHealth { get; private set; } = 100;

    //public event to notify interested listeners of death event and of who died
    public event Action<Health> OnDied;

    //flag to cache dead or alive state
    private bool isDead;

    #region Behaviour Callbacks
    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        CurrentHealth.Value = MaxHealth;

    }
    #endregion

    #region Health System Utility
    public void TakeDamage(int _damage)
    {
        ModifyHealth(-_damage);
    }
    public void RestoreHealth(int _health)
    {
        ModifyHealth(_health);
    }

    public void ModifyHealth(int _value)
    {
        if (isDead) return;

        CurrentHealth.Value += _value;
        CurrentHealth.Value = Mathf.Clamp(CurrentHealth.Value, 0, MaxHealth);

        if (CurrentHealth.Value <= 0)
        {
            isDead = true;
            OnDied?.Invoke(this);
        }
    }
    #endregion
}
