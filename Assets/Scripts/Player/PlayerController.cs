using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{
    private PlayerModel _model;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField] private Transform shootTransform;
    private Animator anim;
    private void Start()
    {
        if (IsOwner)
        {
            _model = GetComponent<PlayerModel>();
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
        var dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        
        _model.Move(dir);
        if (dir != Vector3.zero)
        {
            LookDir(dir);
        }
        Debug.Log(WinCondition.Singleton.alivePlayers.Value);

        if (WinCondition.Singleton.alivePlayers.Value == 1)
        {
            WinCondition.Singleton.DeclareWinner(NetworkManager.Singleton.LocalClientId);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            RequestSpawnBulletServerRpc(playerID);
        }
    }

    
    [ServerRpc (RequireOwnership = false)]
    public void RequestSpawnBulletServerRpc(ulong playerID)
    {
        GameObject bullet = Instantiate(bulletPrefab, shootTransform.position, shootTransform.rotation);
        var netObj = bullet.GetComponent<NetworkObject>();
        netObj.SpawnWithOwnership(playerID);
       // netObj.Despawn(true);
    }

    private void LookDir(Vector3 dir)
    {
        dir.y = 0;
        transform.forward = dir.normalized;
    } 
    
}