using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public class PlayerController : NetworkBehaviour
{
    private PlayerModel _model;

    [SerializeField]
    private GameObject bulletPrefab;
    public NetworkVariable<int> health;

    [SerializeField] private Transform shootTransform;
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float rotationSpeed = 500f;
    private Animator anim;
    private void Start()
    {
        if (IsOwner)
        {
            _model = GetComponent<PlayerModel>();
            health = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        }
        else
        {
            this.enabled = false;
        }

    }
    
    private void Update()
    {
        var playerID = _model.OwnerClientId;

        if (!IsOwner) return;
        var dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _model.Move(dir);
      

        if (Input.GetKeyDown(KeyCode.P))
        {
            RequestSpawnBulletServerRpc(playerID);
        }
    }

    [ServerRpc (RequireOwnership = false)]
    public void RequestSpawnBulletServerRpc(ulong playerID)
    {
       
        GameObject bullet = Instantiate(bulletPrefab, shootTransform.position, shootTransform.rotation);
        bullet.GetComponent<NetworkObject>().SpawnWithOwnership(playerID);
        
        Destroy(bullet, 5f);
    }

    [ServerRpc (RequireOwnership = false)]
    public void TakeDamageServerRpc(int damageAmount)
    {
        health.Value -= damageAmount; 

        
        if(health.Value <= 0)
        {
            Destroy(this.gameObject);
        }
      
    }
}