using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Bullet : NetworkBehaviour
{
    public float speed = 10f;
    private Vector3 direction;
    private Rigidbody _rb;
    private PlayerModel _model;
    private PlayerController playerController;
    private bool _isDestroyed;
    private float LifeTime = 5f;
    private float currentLifeTime;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        if (IsOwner)
        {
            _rb.velocity = _rb.transform.forward * speed;
            currentLifeTime += Time.deltaTime;

            if (currentLifeTime >= LifeTime && !_isDestroyed)
            {
                _isDestroyed = true;
                RequestDestroyServerRPC();
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestDestroyServerRPC()
    {
        var netObj = GetComponent<NetworkObject>();
        netObj.Despawn(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsOwner && other.CompareTag("Player") || other.CompareTag("MovingWall"))
        {
            PlayerModel targetPlayer = other.GetComponent<PlayerModel>();

            if (targetPlayer != null)
            {
                targetPlayer.RequestTakeDamage(10);
                RequestDestroyServerRPC();
            }
            else
            {
                RequestDestroyServerRPC();
            }

        }
    }
}