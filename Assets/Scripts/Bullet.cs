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
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        if (IsOwner)
        {
            _model = GetComponent<PlayerModel>();
        }
        else
        {
            this.enabled = false;
        }
    }
 

    void Update()
    {
        _rb.velocity = _rb.transform.forward * speed;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsOwner && other.CompareTag("Player"))
        {
            // Obtiene el PlayerController del jugador colisionado
            PlayerController targetPlayer = other.GetComponent<PlayerController>();

            if (targetPlayer != null)
            {
                // Llama a un método remoto para restarle vida al jugador
                targetPlayer.TakeDamageServerRpc(10); // Puedes ajustar la cantidad de daño aquí
            }
        }

    }
}