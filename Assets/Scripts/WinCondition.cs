using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class WinCondition : NetworkBehaviour
{
    public GameObject winScreen;
    public GameObject loseScreen;
    public PlayerModel player;
    private void Awake()
    {
        player = GetComponent<PlayerModel>();
    }
    private void Update()
    {
       
            CheckGameOverCondition();
        
    }

    private void CheckGameOverCondition()
    {
        PlayerModel[] players = FindObjectsOfType<PlayerModel>();

        int alivePlayers = 4;
        PlayerModel winner = null;

        foreach (var player in players)
        {
            if (player.currentHealth > 0)
            {
                winner = player;
            }

        }

        if (alivePlayers == 1)
        {
            StartCoroutine(DeclareWinner(winner));
        }
    }

    [ServerRpc]
    public void DeclareLoserServerRPC()
    {
        Debug.Log("derrotaServer");

        DeclareLoserClientRPC();

    }

    [ClientRpc]
    public void DeclareLoserClientRPC()
    {
            Debug.Log("derrotaCliente");
        if (IsOwner && IsClient)
        {

            loseScreen.SetActive(true);

        }


    }

 

    private IEnumerator DeclareWinner(PlayerModel winner)
    {
        DeclareWinnerServerRPC(winner.OwnerClientId);

        yield return new WaitForSeconds(2.0f);

    }

    [ServerRpc]
    private void DeclareWinnerServerRPC(ulong winnerClientId)
    {
        DeclareWinnerClientRPC(winnerClientId);
    }

    [ClientRpc]
    private void DeclareWinnerClientRPC(ulong winnerClientId)
    {
        StartCoroutine(ActivateWinScreen());
    }

    private IEnumerator ActivateWinScreen()
    {
        yield return new WaitForSeconds(0.1f); 
        winScreen.SetActive(true);
    }

}
