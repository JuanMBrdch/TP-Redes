using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class LevelManager : NetworkBehaviour
{

  

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsServer) return;
       
    }
}
