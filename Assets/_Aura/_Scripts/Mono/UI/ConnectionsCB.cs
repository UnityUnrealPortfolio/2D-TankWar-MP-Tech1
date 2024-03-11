using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ConnectionsCB : MonoBehaviour
{
    public void HandleJoinAsHost()
    {
        NetworkManager.Singleton.StartHost();
    }
    public void HandleJoinAsClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}
