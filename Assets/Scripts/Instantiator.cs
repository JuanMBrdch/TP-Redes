using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Instantiator : NetworkBehaviour
{
    public NetworkObject playerPrefab;
    [SerializeField] private Transform zone1;
    [SerializeField] private Transform zone2;
    [SerializeField] private Transform zone3;
    [SerializeField] private Transform zone4;
    
    public void Start()
    {
        ulong id = NetworkManager.Singleton.LocalClientId;
        RequestSpawnPlayerServerRpc(id);
    }
    [ServerRpc(RequireOwnership = false)]
    void RequestSpawnPlayerServerRpc(ulong id)
    {
        var obj = Instantiate<NetworkObject>(playerPrefab);
        obj.SpawnWithOwnership(id);
    }
}