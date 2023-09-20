using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;

public class Timer : NetworkBehaviour
{
    public TMP_Text timerText;
    private float _timer;
    private float _syncTimer;
    public void Start()
    {
        if(IsOwner)
        {
            
        }
    }
    private void Update()
    {
        _timer += Time.deltaTime;
        if (IsServer)
        {
            _syncTimer += Time.deltaTime;
            if (_syncTimer > 3)
            {
                _syncTimer = 0;
                TimerUpdateClientRPC(_timer);
            }
        }
    }

    [ClientRpc]
    private void TimerUpdateClientRPC(float time)
    {
        _timer = time;
    }

    
}
