using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAim : NetworkBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform turretTransform;

    private void LateUpdate()
    {
        if (!IsOwner) return;

        //get mouse pos in world
        var mousePos = Camera.main.ScreenToWorldPoint(inputReader.AimInput);
        //get vector between mouse pos and turret pos
        var direction = new Vector2(mousePos.x - turretTransform.position.x,
            mousePos.y - turretTransform.position.y);
        //turn the turret to that vector
        turretTransform.up = direction;
    }
}
