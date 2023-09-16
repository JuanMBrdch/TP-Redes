using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Bullet : NetworkBehaviour
{
    public float speed = 10f;
    private Vector3 direction;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        
    }
 

    void Update()
    {
        _rb.velocity = _rb.transform.forward * speed;
        
    }

    private void OnTriggerEnter(Collider other)
    {
       // if(Bullet. = PlayerController.PlayerID)
    }
}