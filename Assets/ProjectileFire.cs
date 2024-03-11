using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ProjectileFire : NetworkBehaviour
{
    #region public variable

    [Header("Refs")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private GameObject clientProjectile;
    [SerializeField] private GameObject serverProjectile;

    [Header("Settings")]
    [SerializeField] private float projectileSpeed;

    #endregion

    #region private variables
    private bool shouldFire;
    #endregion

    #region NetworkBehaviour Callbacks
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        inputReader.OnFireAction += HandleFireAction;
    }
    private void Update()
    {
        if (!IsOwner) return;

        if (!shouldFire) return;

        PrimaryFireServerRpc();

        SpawnDummyProjectile();

    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;

        inputReader.OnFireAction -= HandleFireAction;

    }
    #endregion

    #region Input Callbacks
    private void HandleFireAction(bool _shouldFire)
    {
        shouldFire = _shouldFire;
    }
    #endregion

    #region Firing Utility
    private void SpawnDummyProjectile()
    {
        //instantiate a dummy projectile
        var projectile = Instantiate(clientProjectile, projectileSpawnPoint.position, Quaternion.identity);
        //set it's firing direction to face same direction of barrel
        projectile.transform.up = projectileSpawnPoint.up;
    }
    #endregion

    #region Server Code
    [ServerRpc]
    private void PrimaryFireServerRpc()
    {
        //instantiate a dummy projectile
        var projectile = Instantiate(clientProjectile, projectileSpawnPoint.position, Quaternion.identity);
        //set it's firing direction to face same direction of barrel
        projectile.transform.up = projectileSpawnPoint.up;

        PrimaryFireClientRpc();
    }
    #endregion

    #region Client Code
    [ClientRpc]
    private void PrimaryFireClientRpc()
    {
        if (IsOwner) return;

        SpawnDummyProjectile();
    }
    #endregion
}
