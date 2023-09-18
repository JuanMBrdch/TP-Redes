using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public class PlayerController : NetworkBehaviour
{
    private PlayerModel _model;
    private LevelManager _level;
    [SerializeField]
    private GameObject bulletPrefab;
    public NetworkVariable<int> health;
    public NetworkVariable<bool> players;

    [SerializeField] private Transform shootTransform;
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float rotationSpeed = 500f;
    private Animator anim;

    public NetworkVariable<float> gamePlayingTimer; 

    private bool isTimerRunning = false;
    public float localTimer = 0f;
    private float resetUploadTimer = 0f;

    public NetworkVariable<bool> isReadyToStart;

    private void Start()
    {
        if (IsOwner )
        {
            _level = GetComponent<LevelManager>();

            _model = GetComponent<PlayerModel>();
            health = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
            players = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
            gamePlayingTimer = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
            isReadyToStart = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

            if (IsServer )
            {
                StartGamePlayingTimer(); 
            }
        }
        else
        {
            this.enabled = false;
        }

    }
    
    private void Update()
    {
        var playerID = _model.OwnerClientId;


        if (!IsOwner) return;
        Debug.Log("Jugadores " + isReadyToStart.Value);

        if (isReadyToStart.Value == true) {
            var dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            _model.Move(dir);


            if (Input.GetKeyDown(KeyCode.P))
            {
                RequestSpawnBulletServerRpc(playerID);
            }

            if (isTimerRunning)
            {
                localTimer += Time.deltaTime;
                resetUploadTimer += Time.deltaTime;
            }

            if (resetUploadTimer >= 3f)
            {
                UploadLocalTimerServerRpc();
                resetUploadTimer = 0f;
            }
        }

        if (!IsServer) return;

        if(NetworkManager.Singleton.ConnectedClients.Count == 4)
        {
           
                isReadyToStart.Value = true;
            SetPlayersServerRpc(true);

        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void UploadLocalTimerServerRpc()
    {
        gamePlayingTimer.Value = localTimer; 
    }
    [ServerRpc (RequireOwnership = false)]
    public void RequestSpawnBulletServerRpc(ulong playerID)
    {
       
        GameObject bullet = Instantiate(bulletPrefab, shootTransform.position, shootTransform.rotation);
        bullet.GetComponent<NetworkObject>().SpawnWithOwnership(playerID);
        
        Destroy(bullet, 5f);
    }
    [ServerRpc(RequireOwnership = false)]
   public void SetPlayersServerRpc(bool value)
    {

        isReadyToStart.Value = value;

    }
    [ServerRpc (RequireOwnership = false)]
    public void TakeDamageServerRpc(int damageAmount)
    {
        health.Value -= damageAmount; 

        
        if(health.Value <= 0)
        {
            Destroy(this.gameObject);
        }
      
    }
    private void StartGamePlayingTimer()
    {
        isTimerRunning = true;
    }
}