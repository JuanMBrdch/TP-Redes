using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{
    private PlayerModel _model;
    
    
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
        if (!IsOwner) return;
        var dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _model.Move(dir);
    }

    
}
