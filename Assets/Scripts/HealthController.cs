using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class HealthController : NetworkBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    
    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }
    
}
