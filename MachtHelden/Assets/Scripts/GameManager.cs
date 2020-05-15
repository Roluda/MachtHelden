using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Mapbox.Unity.Map;
using Mapbox.Unity.Location;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;
    [SerializeField]
    FireSpawn fireSpawnPrefab;
    [SerializeField]
    PlayerController playerPrefab;

    public bool mapInitialized;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        LocationProviderFactory.Instance.mapManager.OnInitialized += () => mapInitialized = true;
        if (playerPrefab != null)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity,0);
        }
    }

    public void OnEvent(ExitGames.Client.Photon.EventData photonEvent)
    {
        if (photonEvent.Code == FireSpawn.eventCode)
        {
            Debug.Log("FireSpawnerNetworked");
            object[] data = (object[])photonEvent.CustomData;

            FireSpawn newSpawn = Instantiate(fireSpawnPrefab, (Vector3)data[0], (Quaternion)data[1]);
            PhotonView photonView = newSpawn.gameObject.GetComponent<PhotonView>();
            newSpawn.isNetworked = true;
            photonView.ViewID = (int)data[2];
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RequestOwnership();
            }
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Menu");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void QuitApp()
    {
        Application.Quit();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " entered the Game");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + " left the Game");
    }
}
