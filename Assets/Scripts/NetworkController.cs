using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using static Unity.Netcode.NetworkManager;

public class NetworkController : MonoBehaviour
{
    public Button hostButton;
    public Button serverButton;
    public Button clientButton;
    public InputField inputField;
    string _pass;
    public int maxPlayers = 4;
    private void Start()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += ApproveCheck;
        NetworkManager.Singleton.NetworkConfig.ConnectionApproval = true;

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
        NetworkManager.Singleton.OnTransportFailure += OnTransportFailure;
    }
    private void Awake()
    {

    }
    public void OnHost()
    {
        NetworkManager.Singleton.OnServerStarted += OnServerStarted;
        _pass = inputField.text;
        SetInteractable(false);
        NetworkManager.Singleton.StartHost();
    }
    public void OnServer()
    {
        NetworkManager.Singleton.OnServerStarted += OnServerStarted;
        SetInteractable(false);
        _pass = inputField.text;
        NetworkManager.Singleton.StartServer();
    }
    public void OnClient()
    {
        NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.ASCIIEncoding.ASCII.GetBytes(inputField.text);
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectedCallback;

        SetInteractable(false);
        NetworkManager.Singleton.StartClient();
    }
    void LoadScene()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("Level", LoadSceneMode.Single);
    }

    void SetInteractable(bool v)
    {
        hostButton.interactable = v;
        serverButton.interactable = v;
        clientButton.interactable = v;
    }
    void OnServerStarted()
    {
        print("Server preEntrega");
        if (NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;

        }
    }
    void OnClientConnectedCallback(ulong id)
    {
        print("Client connected " + id);
        if (NetworkManager.Singleton.IsServer)
        {
            if(NetworkManager.Singleton.ConnectedClients.Count >= 4)
            {
                LoadScene();

            }
        }
    }
    void OnClientDisconnectedCallback(ulong id)
    {
        if (NetworkManager.Singleton.DisconnectReason == "M")
        {
            Application.Quit();
        }
        else
        {
            print("Client Disconnected " + id + "  " + NetworkManager.Singleton.DisconnectReason);
            SetInteractable(true);
        }
    }
    void OnTransportFailure()
    {
        print("Failure!!!");
        SetInteractable(true);
    }
    void ApproveCheck(ConnectionApprovalRequest request, ConnectionApprovalResponse response)
    {
        var manager = NetworkManager.Singleton;
        if (request.ClientNetworkId == manager.LocalClientId)
        {
            response.Approved = true;
        }
        else
        {
            if (manager.ConnectedClients.Count >= maxPlayers)
            {
                response.Reason = "No podes, genio";
                response.Approved = false;
            }
            else if (System.Text.ASCIIEncoding.ASCII.GetString(request.Payload) != _pass)
            {
                if (System.Text.ASCIIEncoding.ASCII.GetString(request.Payload) == "M")
                {
                    response.Reason = "M";
                }
                else
                {
                    response.Reason = "La pass esta mal, crack";
                }
                response.Approved = false;
            }
            else
            {
                response.Approved = true;
            }
        }
        response.Pending = false;
    }
}
