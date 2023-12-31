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
    public List<Transform> spawnAreas = new List<Transform>();
    private List<Transform> _availableSpawnAreas = new List<Transform>();
    public void Start()
    {
        spawnAreas = new List<Transform>{ zone1, zone2, zone3, zone4 };
        _availableSpawnAreas.AddRange(spawnAreas);
        ulong id = NetworkManager.Singleton.LocalClientId;
        RequestSpawnPlayerServerRpc(id);
    }
    [ServerRpc(RequireOwnership = false)]
    private void RequestSpawnPlayerServerRpc(ulong id)
    {
        if (_availableSpawnAreas.Count > 0)
        {
            int randomIndex = Random.Range(0, _availableSpawnAreas.Count);
            Transform spawnPoint = _availableSpawnAreas[randomIndex];
            var obj = Instantiate<NetworkObject>(playerPrefab, spawnPoint);
            obj.SpawnWithOwnership(id);
            _availableSpawnAreas.RemoveAt(randomIndex);
        }
    }
}