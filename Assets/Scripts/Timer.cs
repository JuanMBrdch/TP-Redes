using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;

public class Timer : NetworkBehaviour
{
    public TMP_Text timerText; 
    private PlayerController _controller;
    public void Start()
    {
        if(IsOwner)
        {
            _controller = GetComponent<PlayerController>();
        }
    }
 

    private void Update()
    {
        if (IsOwner)
        {
            timerText.SetText("Timer: " + _controller.localTimer.ToString());
        }
    }
}
