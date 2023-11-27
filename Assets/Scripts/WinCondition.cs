using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class WinCondition : NetworkBehaviour
{
    private bool gameOver = false;
    public GameObject winScreen;
    public GameObject loseScreen;

    private void Update()
    {
        if (!IsOwner) return;
        if (!gameOver)
        {
            CheckGameOverCondition();
        }
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

            if (player.currentHealth <= 0)
            {
                DeclareLoserServerRPC();
                alivePlayers--;
            }
        }

        if (alivePlayers == 1)
        {
            StartCoroutine(DeclareWinner(winner));
        }
    }

    [ServerRpc]
    private void DeclareLoserServerRPC()
    {
        DeclareLoserClientRPC();
        Debug.Log("derrotaServer");

    }

    [ClientRpc]
    private void DeclareLoserClientRPC()
    {
        StartCoroutine(ActivateLoseScreen());
        Debug.Log("derrotaClient");

    }

    private IEnumerator ActivateLoseScreen()
    {
        yield return new WaitForSeconds(0.1f); 
        if (IsOwner)
        {
            loseScreen.SetActive(true);
        }
    }

    private IEnumerator DeclareWinner(PlayerModel winner)
    {
        DeclareWinnerServerRPC(winner.OwnerClientId);

        yield return new WaitForSeconds(2.0f);

        gameOver = true;
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
        yield return new WaitForSeconds(0.1f); // Ajusta según sea necesario
        winScreen.SetActive(true);
    }

}
