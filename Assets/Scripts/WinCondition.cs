using UnityEngine;
using Unity.Netcode;
public class WinCondition : NetworkBehaviour
{
    public GameObject winScreen;
    public GameObject loseScreen;
    public NetworkVariable<int> alivePlayers;
    
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
            alivePlayers = new NetworkVariable<int>(4, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        }
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
    }
    
    public void ReducePlayerCount()
    {
        alivePlayers.Value --;
    }


    public void DeclareLoser(ulong id)
    {
        loseScreen.SetActive(true);
    }

    // declarar el win desde el server y no de manera local, con un clientRPC
    public void DeclareWinner(ulong id)
    {
        winScreen.SetActive(true);
    }
   
}
