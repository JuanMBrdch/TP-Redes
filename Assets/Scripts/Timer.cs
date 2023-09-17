using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;


public class Timer : NetworkBehaviour
{
    public Text timerText; 
    private PlayerController _controller;
    public void Start()
    {
        if(IsOwner)
        {
            _controller = GetComponent<PlayerController>();
        }
    }
    public void Initialize(NetworkVariable<float> timer)
    {
        _controller.gamePlayingTimer = timer;
    }

    private void Update()
    {
        if (IsOwner && _controller.gamePlayingTimer != null)
        {
            // Actualiza el texto de la UI con el valor del temporizador
            timerText.text = "Timer: " + _controller.gamePlayingTimer.Value.ToString("F2"); 
        }
    }
}
