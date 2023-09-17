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
    
    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }
}
