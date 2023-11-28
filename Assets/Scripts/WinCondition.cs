using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class WinCondition : NetworkBehaviour
{
    public GameObject winScreen;
    public GameObject loseScreen;

    public float alivePlayers = 4;
    private bool  unoVivo = false;
    public static WinCondition Singleton { get; private set; }

    private void Awake()
    {
        if (Singleton != null && Singleton != this)
        {
            Destroy(this);
        }
        else
        {
            Singleton = this;
        }

        winScreen.SetActive(false);
        loseScreen.SetActive(false);
    }
    private void Update()
    {
       
        
    }

    public void CheckGameOverCondition()
    {
        PlayerModel[] players = FindObjectsOfType<PlayerModel>();

       
        
        foreach (var player in players)
        {
            if (player.currentHealth < 0)
            {

            }
              if(alivePlayers == 1)
            {
                unoVivo = true;

            }

        }

     
    }
    public void ReducePlayerCount()
    {
        alivePlayers--;

    }

    [ServerRpc(RequireOwnership = false)]
    public void DeclareLoserServerRPC(ulong id)
    {
        Debug.Log("derrotaServer");

        //DeclareLoserClientRPC(id);

    }

    public void DeclareLoser (ulong id)
    {
        Debug.Log("derrotaCliente");

        // Solo activa la pantalla de derrota en el cliente correspondiente
        
            loseScreen.SetActive(true);
        

    }
    public void DeclareWinner(ulong id)
    {
        Debug.Log("derrotaCliente");
        
            winScreen.SetActive(true);
        
        // Solo activa la pantalla de derrota en el cliente correspondiente



    }
    /* private IEnumerator DeclareWinner(PlayerModel winner)
     {
         DeclareWinnerServerRPC(winner.OwnerClientId);

         yield return new WaitForSeconds(2.0f);

     }*/

    [ServerRpc]
    private void DeclareWinnerServerRPC(ulong winnerClientId)
    {
        DeclareWinnerClientRPC(winnerClientId);
    }

    [ClientRpc]
    private void DeclareWinnerClientRPC(ulong winnerClientId)
    {
        
        winScreen.SetActive(true);
    }

    /* private IEnumerator ActivateWinScreen()
     {
         yield return new WaitForSeconds(0.1f); 
         winScreen.SetActive(true);
     }*/

}
