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
    [SerializeField] private Collider2D ownCollider;
    [SerializeField] private GameObject muzzleFlash;

    [Header("Settings")]
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float fireRate;
    [SerializeField] private float muzzleFlashDuration;
   

    #endregion

    #region private variables
    private bool shouldFire;
    private float fireTimer;
    private float muzzleFlashTimer;

    #endregion

    #region NetworkBehaviour Callbacks
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        inputReader.OnFireAction += HandleFireAction;
    }
    private void Update()
    {
        
        fireTimer -= Time.deltaTime;
        muzzleFlashTimer -=Time.deltaTime;
        if(muzzleFlashTimer < 0 )
        {
            muzzleFlash.SetActive(false);
            
        }

        if (!IsOwner) return;

        if (!shouldFire) return;

        if (fireTimer > 0) return;

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
        //show muzzle flash
        muzzleFlash.SetActive(true);
        muzzleFlashTimer = muzzleFlashDuration;
        //instantiate a dummy projectile
        var projectile = Instantiate(clientProjectile, projectileSpawnPoint.position, Quaternion.identity);
        //set it's firing direction to face same direction of barrel
        projectile.transform.up = projectileSpawnPoint.up;
        //ensure projectile ignores firing actor
        Physics2D.IgnoreCollision(ownCollider,projectile.GetComponent<Collider2D>());
        //access collider rigidbody and apply force
        if(projectile.TryGetComponent(out Rigidbody2D projectileRb))
        {
            projectileRb.velocity = projectile.transform.up * projectileSpeed;
        }
        fireTimer = fireRate;
    }
    #endregion

    #region Server Code
    [ServerRpc]
    private void PrimaryFireServerRpc()
    {
        //show muzzle flash
        muzzleFlash.SetActive(true);
        muzzleFlashTimer = muzzleFlashDuration;

        //instantiate a dummy projectile
        var projectile = Instantiate(serverProjectile, projectileSpawnPoint.position, Quaternion.identity);
        //set it's firing direction to face same direction of barrel
        projectile.transform.up = projectileSpawnPoint.up;
      
        //ensure projectile ignores firing actor
        Physics2D.IgnoreCollision(ownCollider, projectile.GetComponent<Collider2D>());
        //access collider rigidbody and apply force
        if (projectile.TryGetComponent(out Rigidbody2D projectileRb))
        {

            projectileRb.velocity = projectile.transform.up * projectileSpeed;
        }
        fireTimer = fireRate;
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
