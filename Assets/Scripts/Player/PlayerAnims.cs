using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerAnims : NetworkBehaviour
{
    public Animator anim;
    Rigidbody _rb;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        if (!IsOwner)
        {
            enabled = false;
        }
    }

    private void Update()
    {
        if (!IsOwner) return;
        anim.SetFloat("Vel", _rb.velocity.magnitude);
        if (_rb.velocity.magnitude > 0)
        {
            LookDir(_rb.velocity);
        }
    }

    private void LookDir(Vector3 dir)
    {
        dir.y = 0;
        transform.forward = dir.normalized;
    }    
}
