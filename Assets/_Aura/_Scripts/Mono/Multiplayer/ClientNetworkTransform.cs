using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;

public class ClientNetworkTransform : NetworkTransform
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        //allow player to update transform
        CanCommitToTransform = IsOwner;
    }

    protected override bool OnIsServerAuthoritative()
    {
        //disable server authority over this player
        return false;
    }

    protected override void Update()
    {
        CanCommitToTransform = IsOwner;
        base.Update();

        if(NetworkManager.IsClient || NetworkManager.IsListening)
        {
             if(CanCommitToTransform)
            {
                TryCommitTransformToServer(transform, NetworkManager.LocalTime.Time);
            }
        }
    }
}
