using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public class PlayerModel : NetworkBehaviour
{
    private Rigidbody _rb;
    public float speed;

    public int maxHealth;
    public int currentHealth;
    private ClientRpcParams p;
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
        TakeDamage(damage);
        p.Send.TargetClientIds = new ulong[] { OwnerClientId };
        RequestTakeDamageClientRPC(damage, p);
    }

    [ClientRpc]
    private void RequestTakeDamageClientRPC(int damage, ClientRpcParams p)
    {
        TakeDamage(damage);
    }
    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }
}
