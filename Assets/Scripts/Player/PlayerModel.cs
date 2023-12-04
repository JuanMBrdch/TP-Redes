using UnityEngine;
using Unity.Netcode;

public class PlayerModel : NetworkBehaviour
{
    private Rigidbody _rb;
    public float speed;
    public int maxHealth;
    public int currentHealth;
    
    private ClientRpcParams _p;
    private void Awake()
    {
        currentHealth = maxHealth;
        _rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 dir)
    {
        dir *= speed;
        dir.y = _rb.velocity.y;
        _rb.velocity = dir;
    }

    public void RequestTakeDamage(int damage)
    {
        if (IsOwner)
        {
            TakeDamage(damage);
        }
        else
        {
            RequestTakeDamageServerRPC(damage);
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void RequestTakeDamageServerRPC(int damage)
    {
        _p.Send.TargetClientIds = new ulong[] { OwnerClientId };
        RequestTakeDamageClientRPC(damage, _p);
        //var netObj = GetComponent<NetworkObject>();
        //netObj.Despawn();
    }   
    //server rpc meter Despawn y despues ese rpc llamarlo en el TakeDamage
    [ClientRpc]
    private void RequestTakeDamageClientRPC(int damage, ClientRpcParams p)
    {
        TakeDamage(damage);
    }
    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if(currentHealth<= 0 && IsOwner)
        {
            WinCondition.Singleton.DeclareLoser(NetworkManager.Singleton.LocalClientId);
            WinCondition.Singleton.ReducePlayerCount();
            
        }
    }
}
