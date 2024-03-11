using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("Input Reader Ref")]
    [SerializeField] private InputReader inputReader;

    [Header("Movement References")]
    [SerializeField] private Transform bodyTransform;
    [SerializeField] private Rigidbody2D playerRB;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;

    [Tooltip("rate of turn in degrees pa second")]
    [SerializeField] private float turnRate;


    #region private variables
    //cache of last input received from input device
    private Vector2 previousMovementInput;
    #endregion

    #region Network Behaviour Callbacks
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        inputReader.OnMoveAction += HandleOnMoveAction;
    }
    private void Update()
    {
        if (!IsOwner) return;
        float zRot = previousMovementInput.x * -turnRate * Time.deltaTime;
        bodyTransform.Rotate(0f, 0f, zRot);
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        playerRB.velocity = (Vector2)bodyTransform.up * previousMovementInput.y * moveSpeed;
    }
    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;

        inputReader.OnMoveAction -= HandleOnMoveAction;

    }
    #endregion

    #region Callback Handlers
    private void HandleOnMoveAction(Vector2 _input)
    {
        Debug.Log($"receiving {_input}");
        previousMovementInput = _input;
    }
    #endregion
}
