using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Launcher : MonoBehaviourPunCallbacks
{
    [Tooltip("The maximum number of players per Room")]
    [SerializeField]
    private byte maximumPlayersPerRoom = 6;

    [SerializeField]
    ConnectionPanel connectionPanel = null;

    private bool isConnecting;
    private string gameVersion = "1";

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        //var locationCheck = Input.location;
        ExitGames.Client.Photon.PhotonPeer.RegisterType(typeof(FireSpawn), 255, FireSpawn.Serialize, FireSpawn.Deserialize);
        ExitGames.Client.Photon.Hashtable customProps = new ExitGames.Client.Photon.Hashtable();
        customProps.Add("Hero", 0);
        customProps.Add("PowerLevel", 0);
        PhotonNetwork.LocalPlayer.SetCustomProperties(customProps);
    }

    public void Connect()
    {
        isConnecting = true;
        connectionPanel.Hide();
        connectionPanel.Message("Verbinde mit Netzwerk..");
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        connectionPanel.Message("Verbunden mit Server");
        Debug.Log("OnConnectedToMaster called by Launcher");
        if (isConnecting)
        {
            Debug.Log("isConnecting");
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        connectionPanel.Message("Verbindung wurde getrennt");
        Debug.LogWarningFormat("OnDisconnected called by Launcher with reason {0}", cause);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionPanel.Message("Kein Aktiver Raum vorhanden, erstelle neu..");
        Debug.Log("OnJoinRandomFailed called by Launcher. No such room availiable, will create new room");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maximumPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        connectionPanel.Message("Raum wurde betreten");
        Debug.Log("OnJoinedRoom was called by Launcher");
        if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            PhotonNetwork.LoadLevel("Playground");
        }
    }
}
